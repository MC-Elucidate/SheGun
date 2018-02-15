using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetector))]
public class MovementManager : MonoBehaviour {

    private Rigidbody2D playerRigidbody;
    private PlayerStatusManager playerStatus;
    private Animator animator;
    private SpriteRenderer sprite;
    private GroundDetector groundDetector;
    private WallDetector wallclimbDetector;

    [ReadOnly]
    public bool IsGrounded;

    [ReadOnly]
    public EDirection forwardDirection = EDirection.Right;
    
    [HideInInspector]
    public float MovementInput = 0;
    [SerializeField]
    private float regularRunSpeed = 5f;

    private bool HasMovementFreedom;
    private float timeSinceMovementDisabled;
    [SerializeField]
    private float movementDisabledTime = .25f;

    [SerializeField]
    private float JumpPower = 8f;
    [SerializeField]
    private float damageKickbackPower = 5f;

    [SerializeField]
    private float dashSpeed = 15f;
    private float currentDashTime = 0;
    public bool airDashed = false;
    [SerializeField]
    private float maxDashTime = 1;

    public Vector2 RootMotionOffset;
    private Vector3 initialPosition;


    [SerializeField]
    private float maxJumpHoldTime = .25f;
    private float currentJumpHoldTime = 0;
    private bool jumpHeld = false;
    [SerializeField]
    private float jumpGravityScale = 1;
    private float regularGravityScale;

    [SerializeField]
    private float wallslideSpeed = -2;


    private bool CanMove{ get { return playerStatus.playerState == EPlayerState.FreeMovement || playerStatus.playerState == EPlayerState.FreeMoveAttack; } }

    public bool IsMoving { get { return MovementInput != 0; } }

    private bool IsDashing { get { return playerStatus.playerState == EPlayerState.Dashing; } }

    private bool IsExecuting { get { return playerStatus.playerState == EPlayerState.Executing; } }

    private bool IsWallclimbing { get { return wallclimbDetector.IsTouchingWall && forwardDirection == wallclimbDetector.wallDirection && playerStatus.playerState == EPlayerState.FreeMovement; } }

    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStatus = GetComponent<PlayerStatusManager>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        groundDetector = GetComponentInChildren<GroundDetector>();
        wallclimbDetector = GetComponentInChildren<WallDetector>();
        IsGrounded = false;
        HasMovementFreedom = true;
        regularGravityScale = playerRigidbody.gravityScale;
	}
	
	void Update () {
        CheckMovementFreedom();
        CheckIsGrounded();
        CheckIfAirDashed();
        MovementUpdate();
        SetSpriteDirection();
	}

    private void MovementUpdate()
    {
        if (!HasMovementFreedom)
            return;

        if (jumpHeld)
        {
            currentJumpHoldTime += Time.deltaTime;
            if (currentJumpHoldTime > maxJumpHoldTime)
                JumpReleased();
        }

        if (IsDashing)
        {
            playerRigidbody.velocity = DirectionHelper.GetDirectionVector(forwardDirection) * dashSpeed;
            currentDashTime += Time.deltaTime;
            if (currentDashTime > maxDashTime)
                DashReleased();
            return;
        }

        if (jumpHeld)
        {
            currentJumpHoldTime += Time.deltaTime;
            if (currentJumpHoldTime > maxJumpHoldTime)
                JumpReleased();
        }

        if (IsExecuting)
        {
            transform.position = initialPosition + (new Vector3(RootMotionOffset.x * DirectionHelper.GetDirectionVector(forwardDirection).x, RootMotionOffset.y));
        }

        if (!CanMove)
            return;

        if (MovementInput != 0)
        {
            float yVelocity = IsWallclimbing ? wallslideSpeed : playerRigidbody.velocity.y;
            playerRigidbody.velocity = new Vector2(MovementInput * regularRunSpeed, yVelocity);
        }
    }

    private void CheckIsGrounded()
    {
        IsGrounded = groundDetector.isGrounded;
    }

    private void CheckIfAirDashed()
    {
        if (airDashed)
            airDashed = !(IsGrounded || IsWallclimbing);
    }

    public void JumpPressed()
    {
        if ((!IsGrounded || !CanMove) && !IsWallclimbing)
            return;
        
        if (IsWallclimbing)
            playerRigidbody.AddForce(new Vector2(JumpPower * -2 * DirectionHelper.GetDirectionVector(forwardDirection).x, JumpPower));
        else
            playerRigidbody.AddForce(new Vector2(0, JumpPower));
        playerRigidbody.gravityScale = jumpGravityScale;
        jumpHeld = true;
    }

    public void JumpReleased()
    {
        if (!CanMove)
            return;
        playerRigidbody.gravityScale = regularGravityScale;
        jumpHeld = false;
        currentJumpHoldTime = 0;
    }

    public void KickBack(EDirection direction)
    {
        KickBack(direction, damageKickbackPower);
    }
    public void KickBack(EDirection direction, float power)
    {
        switch (direction)
        {
            case EDirection.Down:
            case EDirection.DownLeft:
            case EDirection.DownRight:
                playerRigidbody.velocity = new Vector2(0, power*2);
                break;
        }

        if (!IsGrounded)
        {
            switch (direction)
            {
                case EDirection.Up:
                case EDirection.UpLeft:
                case EDirection.UpRight:
                    playerRigidbody.velocity = new Vector2(0, -power);
                    break;
            }
        }

        switch (direction)
        {
            case EDirection.Left:
                playerRigidbody.velocity = new Vector2(power, 0);
                break;
            case EDirection.UpLeft:
            case EDirection.DownLeft:
                playerRigidbody.velocity = new Vector2(power, playerRigidbody.velocity.y);
                break;
            case EDirection.Right:
                playerRigidbody.velocity = new Vector2(-power, 0);
                break;
            case EDirection.UpRight:
            case EDirection.DownRight:
                playerRigidbody.velocity = new Vector2(-power, playerRigidbody.velocity.y);
                break;
        }
        DisableMovement();
    }

    public void DashPressed()
    {
        if (!CanMove)
            return;
        if (!IsGrounded && airDashed)
            return;

        playerStatus.playerState = EPlayerState.Dashing;
        DisableRigidbodyEffects();
        if (!IsGrounded)
            airDashed = true;
    }
    public void DashReleased()
    {
        if (!IsDashing)
            return;
        playerStatus.playerState = EPlayerState.FreeMovement;
        EnableRigidbodyEffects();
        currentDashTime = 0;
    }

    public void DashInDirectionOf(Transform target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        playerRigidbody.velocity = direction * dashSpeed;
    }

    void LateUpdate()
    {
        animator.SetBool("Dash", IsDashing);
        animator.SetFloat("VelocityX", MovementInput);
        animator.SetFloat("VelocityY", IsGrounded ? 0 : playerRigidbody.velocity.y);
        animator.SetBool("Wallclimb", IsWallclimbing);
    }

    private void SetSpriteDirection()
    {
        if (!CanMove)
            return;

        if (MovementInput < 0 && forwardDirection == EDirection.Right)
        {
            forwardDirection = EDirection.Left;
            FlipSprite();
        }
        else if (MovementInput > 0 && forwardDirection == EDirection.Left)
        {
            forwardDirection = EDirection.Right;
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }

    private void CheckMovementFreedom()
    {
        if (HasMovementFreedom)
            return;

        if (timeSinceMovementDisabled > movementDisabledTime)
            HasMovementFreedom = true;
        else
            timeSinceMovementDisabled += Time.deltaTime;
    }

    private void DisableMovement()
    {
        HasMovementFreedom = false;
        timeSinceMovementDisabled = 0;
    }

    public void DisableRigidbodyEffects()
    {
        playerRigidbody.velocity = new Vector2(0, 0);
        playerRigidbody.gravityScale = 0;
    }

    public void EnableRigidbodyEffects()
    {
        playerRigidbody.gravityScale = regularGravityScale;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == Constants.Tags.Enemy)
        {
            EnemyStatusManager enemy = other.gameObject.GetComponent<EnemyStatusManager>();
            if (enemy.IsExecutable() && IsDashing)
            {
                animator.SetTrigger("Execute");
                initialPosition = transform.position;
                DisableRigidbodyEffects();
                enemy.ReceiveMeleeDamage(100);
                airDashed = false;
                playerStatus.RegenerateOnExecute(enemy);
                playerStatus.playerState = EPlayerState.Executing;
            }
            else
                enemy.CollideWithPlayer();
        }
    }

    public void EndExecution()
    {
        playerStatus.playerState = EPlayerState.Dashing;
        DashReleased();
    }
}

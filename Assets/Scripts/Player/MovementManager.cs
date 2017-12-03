using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementManager : MonoBehaviour {

    private Rigidbody2D playerRigidbody;
    private PlayerStatusManager playerStatus;
    private Animator animator;
    private SpriteRenderer sprite;

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

    private float regularGravityScale;

    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStatus = GetComponent<PlayerStatusManager>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        IsGrounded = false;
        HasMovementFreedom = true;
        regularGravityScale = playerRigidbody.gravityScale;
	}
	
	void Update () {
        CheckMovementFreedom();
        CheckIsGrounded();
        MovementUpdate();
        SetSpriteDirection();
	}

    private void MovementUpdate()
    {
        if (!HasMovementFreedom)
            return;

        if (MovementInput != 0 && HasMovementFreedom)
            playerRigidbody.velocity = new Vector2(MovementInput * regularRunSpeed, playerRigidbody.velocity.y);
    }

    private void CheckIsGrounded()
    {
        IsGrounded = Mathf.Abs(playerRigidbody.velocity.y) < 0.0001;
    }

    public void Jump()
    {
        if (!IsGrounded)
            return;
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, JumpPower);
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

    public void DashInDirectionOf(Transform target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        playerRigidbody.velocity = direction * dashSpeed;
    }

    void LateUpdate()
    {
        animator.SetFloat("VelocityX", playerRigidbody.velocity.x);
        animator.SetFloat("VelocityY", playerRigidbody.velocity.y);
    }

    private void SetSpriteDirection()
    {
        if (playerRigidbody.velocity.x < 0 && forwardDirection == EDirection.Right)
        {
            forwardDirection = EDirection.Left;
            FlipSprite();
        }
        else if (playerRigidbody.velocity.x > 0 && forwardDirection == EDirection.Left)
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

    public void StartGunDatsuMovement()
    {
        playerRigidbody.velocity = new Vector2(0, 0);
        playerRigidbody.gravityScale = 0;
    }

    public void EndGunDatsuMovement()
    {
        playerRigidbody.gravityScale = regularGravityScale;
    }
}

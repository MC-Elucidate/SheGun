using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementManager : MonoBehaviour {

    private Rigidbody2D playerRigidbody;
    private PlayerStatusManager playerStatus;
    public bool IsGrounded { get; private set; }
    public float MovementInput = 0;
    public float regularRunSpeed = 10f;
    public float runInputForce = 2f;
    
    [SerializeField]
    private float JumpPower = 8f;
    [SerializeField]
    private float collisionKickback = 5f;
    [SerializeField]
    private float dashSpeed = 15f;

    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStatus = GetComponent<PlayerStatusManager>();
        IsGrounded = false;
	}
	
	void Update () {
        CheckIsGrounded();
        MovementUpdate();
	}

    private void MovementUpdate()
    {
        if (MovementInput != 0)
        {
            if (Mathf.Abs(playerRigidbody.velocity.x) < regularRunSpeed)
            {
                playerRigidbody.AddForce(new Vector2(runInputForce * MovementInput, 0), ForceMode2D.Impulse);
            }
        }
    }

    private void CheckIsGrounded()
    {
        IsGrounded = Mathf.Abs(playerRigidbody.velocity.y) < 0.0001;
        playerRigidbody.drag = IsGrounded ? 5 : 0;
        runInputForce = IsGrounded ? 2 : 0.1f;
    }

    public void Jump()
    {
        if (!IsGrounded)
            return;
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, JumpPower);

    }

    public void KickBack(EDirection direction, float power)
    {
        switch (direction)
        {
            case EDirection.Down:
            case EDirection.DownLeft:
            case EDirection.DownRight:
                playerRigidbody.velocity = new Vector2(0, power);
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
            case EDirection.UpLeft:
            case EDirection.DownLeft:
                playerRigidbody.velocity = new Vector2(power, playerRigidbody.velocity.y);
                break;
            case EDirection.Right:
            case EDirection.UpRight:
            case EDirection.DownRight:
                playerRigidbody.velocity = new Vector2(-power, playerRigidbody.velocity.y);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerStatus.ReceiveDamage(1);
            EDirection directionOfContact = collision.transform.position.x >= transform.position.x ? EDirection.DownRight : EDirection.DownLeft;
            KickBack(directionOfContact, collisionKickback);
        }
    }

    public void DashInDirectionOf(Transform target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        playerRigidbody.velocity = direction * dashSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour {

    [SerializeField]
    private int slashDamage = 5;
    [SerializeField]
    private int airSlashDamage = 10;

    [SerializeField]
    private AudioClip slashSound;

    private AudioSource audioSource;
    private MeleeHitbox meleeHitbox;


    private int currentAttackNumber = 0;
    private int maxAttackNumber = 3;
    private PlayerStatusManager playerStatus;
    private MovementManager movementManager;
    private Animator animator;

    void Start () {
        meleeHitbox = GetComponentInChildren<MeleeHitbox>();
        audioSource = GetComponent<AudioSource>();
        playerStatus = GetComponent<PlayerStatusManager>();
        movementManager = GetComponent<MovementManager>();
        animator = GetComponent<Animator>();
	}
	
	void Update () {
		
	}

    public void AttackPressed()
    {
        //switch (direction)
        //{
        //    case EDirection.Down:
        //    case EDirection.DownLeft:
        //    case EDirection.DownRight:
        //        LaunchAttack();
        //        break;
        //    default:
        //        SlashAttack();
        //        break;
        //}

       // print(movementManager.IsGrounded);
        if (movementManager.IsGrounded)
        {
            if (movementManager.IsMoving)
            {
                RunAttack();
                return;
            }
            ++currentAttackNumber;
            if (currentAttackNumber > maxAttackNumber)
                return;
            SlashAttack();
        }
        else
        {
            AirAttack();
        }
    }

    private void SlashAttack()
    {
        playerStatus.playerState = EPlayerState.Attacking;
        animator.SetTrigger("Attack");
        meleeHitbox.AttackEnemies(slashDamage);
        audioSource.PlayOneShot(slashSound);
    }

    private void AirAttack()
    {
        if (playerStatus.playerState == EPlayerState.FreeMoveAttack)
            return;
        playerStatus.playerState = EPlayerState.FreeMoveAttack;
        animator.SetTrigger("Attack");
        meleeHitbox.AttackEnemies(airSlashDamage);
        audioSource.PlayOneShot(slashSound);
    }

    private void RunAttack()
    {
        if (playerStatus.playerState == EPlayerState.FreeMoveAttack)
            return;
        playerStatus.playerState = EPlayerState.FreeMoveAttack;
        animator.SetTrigger("Attack");
        meleeHitbox.AttackEnemies(slashDamage);
        audioSource.PlayOneShot(slashSound);
    }

    public void EndAttack()
    {
        playerStatus.playerState = EPlayerState.FreeMovement;
        currentAttackNumber = 0;
    }
}

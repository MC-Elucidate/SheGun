using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour {

    [SerializeField]
    private int slashDamage = 1;

    [SerializeField]
    private AudioClip slashSound;

    private AudioSource audioSource;
    private MeleeHitbox meleeHitbox;


    private int currentAttackNumber = 0;
    private int maxAttackNumber = 3;
    private PlayerStatusManager playerStatus;
    private Animator animator;

    void Start () {
        meleeHitbox = GetComponentInChildren<MeleeHitbox>();
        audioSource = GetComponent<AudioSource>();
        playerStatus = GetComponent<PlayerStatusManager>();
        animator = GetComponent<Animator>();
	}
	
	void Update () {
		
	}

    public void AttackPressed(EDirection direction)
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
        ++currentAttackNumber;
        if (currentAttackNumber > maxAttackNumber)
            return;
        SlashAttack();
        audioSource.PlayOneShot(slashSound);
    }

    private void SlashAttack()
    {
        playerStatus.playerState = EPlayerState.Attacking;
        animator.SetTrigger("Attack");
        meleeHitbox.AttackEnemies(slashDamage);
    }

    private void EndAttack()
    {
        playerStatus.playerState = EPlayerState.FreeMovement;
        currentAttackNumber = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour {

    [SerializeField]
    private int slashDamage = 1;

    [SerializeField]
    private int launchDamage = 0;

    [SerializeField]
    private AudioClip slashSound;

    private AudioSource audioSource;
    private MeleeHitbox meleeHitbox;

    void Start () {
        meleeHitbox = GetComponentInChildren<MeleeHitbox>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
		
	}

    public void AttackPressed(EDirection direction)
    {
        switch (direction)
        {
            case EDirection.Down:
            case EDirection.DownLeft:
            case EDirection.DownRight:
                LaunchAttack();
                break;
            default:
                SlashAttack();
                break;
        }
        audioSource.PlayOneShot(slashSound);
    }

    private void LaunchAttack()
    {
        meleeHitbox.AttackEnemies(true, launchDamage);
    }

    private void SlashAttack()
    {
        meleeHitbox.AttackEnemies(false, slashDamage);
    }
}

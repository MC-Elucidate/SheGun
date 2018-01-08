using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShieldEnemyStatusManager : EnemyStatusManager, IEntityWithWeakpoint {

    [SerializeField]
    private Color vulnerableColour = Color.white;
    [SerializeField]
    private int maxShieldHealth = 3;
    [SerializeField]
    private int currentShieldHealth;
    [SerializeField]
    private int meleeDamage = 2;
    [SerializeField]
    private GameObject fallingProjectile;

    private EnemyMeleeHitbox meleeHitbox;
    public bool attacking = false;
    

    protected override void Start()
    {
        base.Start();
        currentShieldHealth = maxShieldHealth;
        //StartCoroutine(AttackLoop());
        meleeHitbox = GetComponentInChildren<EnemyMeleeHitbox>();
        meleeHitbox.playerStatus = playerStatus;
    }

    protected override void Update()
    {
       base.Update();
       attacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    public override bool ReceiveBulletDamage(int damage)
    {
        if (currentShieldHealth > 0)
            return false;

        return base.ReceiveBulletDamage(damage);
    }

    public override void ReceiveMeleeDamage(int damage)
    {
        if (currentShieldHealth > 0)
        {
            currentShieldHealth -= damage;
            base.ReceiveMeleeDamage(damage);

            if (currentShieldHealth <= 0)
                defaultColour = vulnerableColour;
        }
        else
            base.ReceiveMeleeDamage(damage);
    }

    private IEnumerator AttackLoop()
    {
        while (Health > 0)
        {
            if (!attacking)
            {
                yield return new WaitForSecondsRealtime(3f);

                if ((player.transform.position - transform.position).sqrMagnitude > 50)
                    continue;
                animator.SetTrigger("Attack");
                
            }
        }
    }

    private void AttackCast()
    {
        meleeHitbox.BeginAttack(meleeDamage);
    }

    private void SummonFallingProjectileOverPlayer()
    {
        Instantiate(fallingProjectile, player.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
    }

    private void EndAttack()
    {
        meleeHitbox.EndAttack();
    }

    public void WeakpointHit(WeakPoint weakpointThatWasHit)
    {
        print("ITAI!");
    }
}

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
    [SerializeField]
    private float timeBetweenAttacks = 3f;
    private float timeSinceLastAttack = 0;
    [SerializeField]
    private float attackStateRange = 50;


    private EnemyMeleeHitbox meleeHitbox;
    public bool attacking = false;
    

    protected override void Start()
    {
        base.Start();
        currentShieldHealth = maxShieldHealth;
        meleeHitbox = GetComponentInChildren<EnemyMeleeHitbox>();
        meleeHitbox.playerStatus = playerStatus;
    }

    protected override void Update()
    {
       base.Update();
       AttackLoop();
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
            base.ReceiveMeleeDamage(1);

            if (currentShieldHealth <= 0)
                defaultColour = vulnerableColour;
        }
        else
            base.ReceiveMeleeDamage(damage);
    }

    private void AttackLoop()
    {
        if ((player.transform.position - transform.position).sqrMagnitude > attackStateRange)
            return;

        if (timeSinceLastAttack < timeBetweenAttacks)
        {
            timeSinceLastAttack += Time.deltaTime;
            return;
        }

        if (!attacking && Health > 0)
        {
            attacking = true;
            animator.SetTrigger("Attack");
            timeSinceLastAttack = 0;
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
        attacking = false;
        meleeHitbox.EndAttack();
    }

    public void WeakpointHit(WeakPoint weakpointThatWasHit)
    {
        print("ITAI!");
    }

    public override void CollideWithPlayer()
    {
    }
}

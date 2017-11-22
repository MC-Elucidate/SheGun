using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShieldEnemyStatusManager : EnemyStatusManager {

    [SerializeField]
    private Color vulnerableColour = Color.white;
    [SerializeField]
    private int maxShieldHealth = 3;
    [SerializeField]
    private int currentShieldHealth;
    [SerializeField]
    private int meleeDamage = 2;
    [SerializeField]
    private int desperationThreshold = 5;
    [SerializeField]
    private GameObject desperationSignal;
    [SerializeField]
    private GameObject fallingProjectile;

    private EnemyMeleeHitbox meleeHitbox;
    private EDirection forwardDirection = EDirection.Left;
    public bool attacking = false;
    

    protected override void Start()
    {
        base.Start();
        currentShieldHealth = maxShieldHealth;
        StartCoroutine(AttackLoop());
        meleeHitbox = GetComponentInChildren<EnemyMeleeHitbox>();
        meleeHitbox.playerStatus = playerStatus;
    }

    void Update()
    {
       attacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
       SetSpriteDirection();
    }

    public override bool ReceiveBulletDamage(int damage)
    {
        if (currentShieldHealth > 0)
            return false;

        return base.ReceiveBulletDamage(damage);
    }

    public override void ReceiveMeleeDamage(bool launching, int damage)
    {
        if (currentShieldHealth > 0)
        {
            currentShieldHealth -= damage;
            base.ReceiveMeleeDamage(launching, damage);

            if (currentShieldHealth <= 0)
                defaultColour = vulnerableColour;
        }
        else
            base.ReceiveMeleeDamage(launching, damage);
    }

    private IEnumerator AttackLoop()
    {
        while (Health > 0)
        {
            if (!attacking)
            {
                yield return new WaitForSecondsRealtime(3f);
                if (Random.Range(1, 3) > 2)
                    SummonFallingProjectileOverPlayer();
                else
                {
                    if (Health <= desperationThreshold)
                    {
                        SignalDesperationAttack();
                        yield return new WaitForSecondsRealtime(1f);
                    }
                    animator.SetTrigger("Attack");
                }
            }
        }
    }

    private void AttackCast()
    {
        meleeHitbox.BeginAttack(meleeDamage);
    }

    public override bool PerformingDesperationAttack()
    {
        return (Health <= desperationThreshold) && attacking;
    }

    private void SignalDesperationAttack()
    {
        GameObject signal = Instantiate(desperationSignal, transform.position, Quaternion.identity);
        Destroy(signal, 1f);
    }

    private void SummonFallingProjectileOverPlayer()
    {
        Instantiate(fallingProjectile, player.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
    }

    private void SetSpriteDirection()
    {
        if (player.transform.position.x < transform.position.x && forwardDirection == EDirection.Right)
        {
            FlipSprite();
            forwardDirection = EDirection.Left;
        }
        else if (player.transform.position.x > transform.position.x && forwardDirection == EDirection.Left)
        {
            forwardDirection = EDirection.Right;
            FlipSprite();
        }
            
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }

    private void EndAttack()
    {
        print("fok");
        meleeHitbox.EndAttack();
    }
}

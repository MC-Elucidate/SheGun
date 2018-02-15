using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyStatusManager : EnemyStatusManager {

    [SerializeField]
    private bool chasingType = false;

    [ReadOnly]
    private bool isChasingPlayer = false;

    [SerializeField]
    private float triggerRange;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int collisionDamage;

    protected override void Start()
    {
        base.Start();
        sprite.color = Color.green;
    }

    protected override void Update()
    {
        base.Update();
        CheckPlayerRange();
        SetVelocity();
        UpdateAnimator();
    }

    private void CheckPlayerRange()
    {
        if (!chasingType)
            return;

        if (Health <= 0)
        {
            isChasingPlayer = false;
            return;
        }
            if ((player.transform.position - transform.position).sqrMagnitude <= triggerRange)
        {
            if (!isChasingPlayer)
                isChasingPlayer = true;
        }
        else
            isChasingPlayer = false;
    }

    private void SetVelocity()
    {
        if (isChasingPlayer)
            enemyRigidbody.velocity = (player.transform.position - transform.position).normalized * moveSpeed;
        else
            enemyRigidbody.velocity = new Vector3();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Chasing", isChasingPlayer);
    }

    public override void CollideWithPlayer()
    {
        if (Health <= 0)
            return;
        playerStatus.ReceiveDamage(collisionDamage, transform);
        ReceiveMeleeDamage(100);
    }
}

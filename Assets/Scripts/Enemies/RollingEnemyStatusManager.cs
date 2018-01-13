using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingEnemyStatusManager : EnemyStatusManager {

    [SerializeField]
    private EDirection moveDirection = EDirection.Left;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int collisionDamage;

    protected override void Start () {
        base.Start();
	}

    protected override void Update()
    {
        base.Update();
        if (Health > 0)
            enemyRigidbody.velocity = (DirectionHelper.GetDirectionVector(moveDirection) * moveSpeed) + new Vector2(0, enemyRigidbody.velocity.y);
        else
            enemyRigidbody.velocity = new Vector2();
    }


    public override void CollideWithPlayer()
    {
        if (Health <= 0)
            return;
        playerStatus.ReceiveDamage(collisionDamage, transform);
        ReceiveMeleeDamage(100);
    }
}

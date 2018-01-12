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
        enemyRigidbody.velocity = (DirectionHelper.GetDirectionVector(moveDirection) * moveSpeed) + new Vector2(0, enemyRigidbody.velocity.y);
    }


    public override void CollideWithPlayer()
    {
        playerStatus.ReceiveDamage(collisionDamage, transform);
        ReceiveMeleeDamage(100);
    }
}

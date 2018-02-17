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
    [SerializeField]
    private float lifespan;

    private WallDetector wallDetector;

    protected override void Start() {
        base.Start();
        wallDetector = GetComponentInChildren<WallDetector>();
        StartCoroutine(Expire());
    }

    protected override void Update()
    {
        base.Update();
        CheckWallDetection();
        UpdateVelocity();

    }


    private void UpdateVelocity()
    {
        if (Health > 0)
            enemyRigidbody.velocity = (DirectionHelper.GetDirectionVector(moveDirection) * moveSpeed) + new Vector2(0, enemyRigidbody.velocity.y);
        else
            enemyRigidbody.velocity = new Vector2();
    }

    private void CheckWallDetection()
    {
        if (wallDetector.IsTouchingWall && wallDetector.wallDirection == moveDirection)
        {
            moveDirection = moveDirection == EDirection.Left ? EDirection.Right : EDirection.Left;
        }
    }


    public override void CollideWithPlayer()
    {
        if (Health <= 0)
            return;
        playerStatus.ReceiveDamage(collisionDamage, transform);
        ReceiveMeleeDamage(100);
    }

    private IEnumerator Expire()
    {
        yield return new WaitForSecondsRealtime(lifespan);
        TakeDamage(100);
    }

}

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
    [SerializeField]
    private float explodeTimer;
    [SerializeField]
    private float explosionRadius;
    [SerializeField]
    Vector2 executeLaunchForce;
    [SerializeField]
    GameObject explosionEffect;

    private bool executed = false;

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

        if (executed)
            return;

        if (Health > 0)
            enemyRigidbody.velocity = (DirectionHelper.GetDirectionVector(moveDirection) * moveSpeed) + new Vector2(0, enemyRigidbody.velocity.y);
        else
            enemyRigidbody.velocity = new Vector2();
    }

    private void CheckWallDetection()
    {
        if (executed)
            return;
        if (wallDetector.IsTouchingWall && wallDetector.wallDirection == moveDirection)
        {
            moveDirection = moveDirection == EDirection.Left ? EDirection.Right : EDirection.Left;
        }
    }


    public override void CollideWithPlayer()
    {
        if (executed)
            return;

        if (Health <= 0)
            return;
        playerStatus.ReceiveDamage(collisionDamage, transform);
        ReceiveMeleeDamage(100);
    }

    private IEnumerator Expire()
    {
        yield return new WaitForSecondsRealtime(lifespan);

        if (executed)
            yield break;

        TakeDamage(100);
    }

    public override void Executed()
    {
        executed = true;
        int launchDirectionMultiplier = (player.transform.position - transform.position).x < 0 ? 1 : -1;
        enemyRigidbody.velocity = Vector2.zero;
        enemyRigidbody.AddForce(new Vector2(executeLaunchForce.x * launchDirectionMultiplier, executeLaunchForce.y));
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSecondsRealtime(explodeTimer);
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        for (int i = 0; i < collidersHit.Length; i++)
        {
            if (collidersHit[i].GetComponent<ExplodableObject>() != null)
                Destroy(collidersHit[i].gameObject);
        }

        Destroy(explosion, 2);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, .5f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}

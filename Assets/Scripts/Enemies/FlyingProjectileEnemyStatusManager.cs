using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingProjectileEnemyStatusManager : EnemyStatusManager
{

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float timeBetweenAttacks = 2f;

    [ReadOnly]
    private bool chasing = false;

    [SerializeField]
    private float triggerRange;
    [SerializeField]
    private float stoppingRange;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 offsetFromPlayer;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(FireProjectile());
    }

    protected override void Update()
    {
        base.Update();
        CheckPlayerRange();
        UpdateAnimator();
    }

    protected void FixedUpdate()
    {
        SetVelocity();
    }

    private void CheckPlayerRange()
    {
        if (Health <= 0)
        {
            chasing = false;
            return;
        }

        float distanceFromPlayer = (player.transform.position - transform.position).magnitude;
        if (distanceFromPlayer <= triggerRange)
        {
            if (!chasing)
                chasing = true;
        }
    }

    private void SetVelocity()
    {
        if (chasing)
        {
            Vector3 destination = player.transform.position + offsetFromPlayer;

            if ((destination - transform.position).magnitude > stoppingRange)
            {
                enemyRigidbody.velocity = ((destination - transform.position).normalized * moveSpeed);
            }
            else
                enemyRigidbody.velocity = Vector2.zero;

        }
        else
            enemyRigidbody.velocity = new Vector3();
    }

    private void UpdateAnimator()
    {
        //animator.SetBool("Chasing", chasing);
    }

    public override void CollideWithPlayer()
    {
    }

    private IEnumerator FireProjectile()
    {
        while (Health > 0)
        {
            yield return new WaitForSecondsRealtime(timeBetweenAttacks);
            if (!chasing)
                continue;
            EnemyProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            projectile.TargetPosition = player.transform.position;
        }
    }
}

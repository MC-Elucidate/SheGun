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

    private PlayerSensor playerSensor;

    protected override void Start()
    {
        base.Start();
        playerSensor = GetComponentInChildren<PlayerSensor>();
        StartCoroutine(FireProjectile());
    }

    protected override void Update()
    {
        base.Update();
        UpdateAnimator();
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
            if (!playerSensor.playerInRange)
                continue;
            EnemyProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            projectile.TargetPosition = player.transform.position;
        }
    }
}

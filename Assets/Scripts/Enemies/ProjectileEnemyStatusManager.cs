using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyStatusManager : EnemyStatusManager {

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float timeBetweenAttacks = 2f;

    private PlayerSensor playerSensor;

    protected override void Start()
    {
        base.Start();
        playerSensor = GetComponentInChildren<PlayerSensor>();
        StartCoroutine(FireProjectile());
    }

    private IEnumerator FireProjectile()
    {
        while (Health > 0)
        {
            yield return new WaitForSecondsRealtime(timeBetweenAttacks);
            if (!playerSensor.playerInRange)
                continue;
            EnemyProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            Vector2 playerDirection = DirectionHelper.GetDirectionVector((player.transform.position - transform.position).x > 0 ? EDirection.Right : EDirection.Left);
            print(playerDirection);
            projectile.TargetPosition = transform.position + new Vector3(playerDirection.x, playerDirection.y, 0);
        }
    }

    public override void CollideWithPlayer()
    {
    }
}

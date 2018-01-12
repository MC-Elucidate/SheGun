using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyStatusManager : EnemyStatusManager {

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float timeBetweenAttacks = 2f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(FireProjectile());
    }

    private IEnumerator FireProjectile()
    {
        while (Health > 0)
        {
            yield return new WaitForSecondsRealtime(timeBetweenAttacks);
            EnemyProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            projectile.TargetPosition = player.transform.position;
        }
    }

    public override void CollideWithPlayer()
    {
    }
}

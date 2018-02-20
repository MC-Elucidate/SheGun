using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingEnemyDispenserEnemyStatusManager : EnemyStatusManager, IEntityWithWeakpoint {

    [SerializeField]
    private float timeBetweenDispense;
    private float timeSinceDispense = 0;

    [ReadOnly]
    [SerializeField]
    private bool dispensing = false;

    [SerializeField]
    private GameObject rollingEnemyPrefab;

    private PlayerSensor playerSensor;

    protected override void Start()
    {
        base.Start();
        playerSensor = GetComponentInChildren<PlayerSensor>();
    }

    protected override void Update()
    {
        base.Update();
        CheckPlayerSensor();
        CheckDispenseTimer();
    }

    private void CheckPlayerSensor()
    {
        dispensing = playerSensor.playerInRange;
    }

    private void CheckDispenseTimer()
    {
        if (!dispensing)
            return;

        if (timeSinceDispense >= timeBetweenDispense)
            Dispense();
        else
            timeSinceDispense += Time.deltaTime;
    }

    private void Dispense()
    {
        timeSinceDispense = 0;
        RollingEnemyStatusManager enemy = Instantiate(rollingEnemyPrefab, transform.position, Quaternion.identity).GetComponent<RollingEnemyStatusManager>();
        enemy.moveDirection = forwardDirection;
        animator.SetTrigger("Dispense");
    }

    public override void CollideWithPlayer()
    {
    }

    public void WeakpointHit(WeakPoint weakpointThatWasHit)
    {
        this.ReceiveBulletDamage(100);
    }
}

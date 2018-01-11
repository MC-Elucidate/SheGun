using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyStatusManager : EnemyStatusManager {

    [ReadOnly]
    private bool chasing = false;

    [SerializeField]
    private float triggerRange;
    [SerializeField]
    private float moveSpeed;

    private new Rigidbody2D rigidbody;
    private new Animator animator;

    protected override void Start()
    {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        if ((player.transform.position - transform.position).sqrMagnitude <= triggerRange)
        {
            if (!chasing)
                chasing = true;
        }
        else
            chasing = false;
    }

    private void SetVelocity()
    {
        if (chasing)
            rigidbody.velocity = (player.transform.position - transform.position).normalized * moveSpeed;
        else
            rigidbody.velocity = new Vector3();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Chasing", chasing);
    }
}

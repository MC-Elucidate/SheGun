using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspendedPlatform : MonoBehaviour, IEntityWithWeakpoint {

    [SerializeField]
    private int numberOfSuspenders;

    private new Rigidbody2D rigidbody;

    public void WeakpointHit(WeakPoint weakpointThatWasHit)
    {
        numberOfSuspenders -= 1;

        if (numberOfSuspenders == 0)
            ReleaseSuspension();
    }

    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void ReleaseSuspension()
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
	
}

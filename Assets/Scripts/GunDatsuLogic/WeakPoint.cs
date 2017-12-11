using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour {

    private IEntityWithWeakpoint entityWithWeakpoint;
    void Start()
    {
        entityWithWeakpoint = GetComponentInParent<IEntityWithWeakpoint>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Constants.Tags.GunDatsuBullet)
        {
            entityWithWeakpoint.WeakpointHit(this);
            Destroy(gameObject);
        }
    }
}

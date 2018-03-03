using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockZone : MonoBehaviour {

    [SerializeField]
    bool LockX;
    [SerializeField]
    bool LockY;

    void OnTriggerEnter2D(Collider2D other)
    {
        CameraManager cam = Camera.main.GetComponent<CameraManager>();
        cam.ChangeXLock(LockX, transform.position.x);
        cam.ChangeYLock(LockY, transform.position.y);
    }
}

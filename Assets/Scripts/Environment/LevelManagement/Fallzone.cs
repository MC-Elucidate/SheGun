using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallzone : MonoBehaviour {

    [SerializeField]
    private Vector3 recoveryPosition;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == Constants.Tags.Player)
            collider.gameObject.GetComponent<PlayerStatusManager>().OutOfBounds(recoveryPosition);
    }
}

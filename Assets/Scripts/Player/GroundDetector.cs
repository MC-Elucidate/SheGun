using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour {

    [ReadOnly]
    public bool isGrounded = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        isGrounded = true;
    }

    void OnTriggerExit2D()
    {
        isGrounded = false;
    }

}

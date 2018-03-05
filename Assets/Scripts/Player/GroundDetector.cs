using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour {

    [ReadOnly]
    public bool isGrounded = false;

    [ReadOnly]
    public int numberOfGrounds = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        ++numberOfGrounds;
        isGrounded = true;
    }

    void OnTriggerExit2D()
    {
        --numberOfGrounds;
        if(numberOfGrounds==0)
            isGrounded = false;
    }

}

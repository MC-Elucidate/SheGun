using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour {

    [ReadOnly]
    public bool playerInRange; 

    void OnTriggerEnter2D(Collider2D other)
    {
        playerInRange = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        playerInRange = false;
    }
}

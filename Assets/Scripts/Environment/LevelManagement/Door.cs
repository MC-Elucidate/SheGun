using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField]
    private GameObject connectedDoor;

    private GameObject player;

    private bool playerInRange = false;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (playerInRange)
            if (Input.GetButtonDown("Interact"))
                player.transform.position = new Vector3(connectedDoor.transform.position.x, player.transform.position.y, player.transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            playerInRange = false;
    }
}

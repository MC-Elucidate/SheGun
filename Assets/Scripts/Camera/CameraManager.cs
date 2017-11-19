using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Transform playerTransform;

	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;	
	}
	
	void Update () {
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Transform playerTransform;

    [SerializeField]
    private float yOffset;

	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player).transform;
	}
	
	void Update () {
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, transform.position.z);
	}
}

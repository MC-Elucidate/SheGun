using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Transform playerTransform;
    private new Camera camera;
    private int defaultMask;
    private int targetMask;

    [SerializeField]
    private float yOffset;

    [SerializeField]
    private bool yLock;
    [SerializeField]
    private bool xLock;

    void Start () {
        playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player).transform;
        camera = GetComponent<Camera>();
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, transform.position.z);
    }
	
	void Update () {
        float newX, newY;

        if (xLock)
            newX = transform.position.x;
        else
            newX = playerTransform.position.x;

        if (yLock)
            newY = transform.position.y;
        else
            newY = playerTransform.position.y + yOffset;

        
        this.transform.position = new Vector3(newX, newY, transform.position.z);
	}
}

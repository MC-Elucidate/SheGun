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
    private float smoothTime;

    [SerializeField]
    public bool yLock;
    [SerializeField]
    public bool xLock;

    private float xLockPos;
    private float yLockPos;

    void Start () {
        playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player).transform;
        camera = GetComponent<Camera>();
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, transform.position.z);
    }
	
	void LateUpdate () {
        float newX, newY;

        if (xLock)
            newX = xLockPos;
        else
            newX = playerTransform.position.x;

        if (yLock)
            newY = yLockPos + yOffset;
        else
            newY = playerTransform.position.y + yOffset;


        Vector3 velocity = Vector3.zero;
        this.transform.position = Vector3.SmoothDamp(transform.position, new Vector3(newX, newY, transform.position.z), ref velocity, smoothTime);
    }

    public void ChangeXLock(bool shouldLock, float position)
    {
        if (shouldLock == xLock)
            return;

        if (shouldLock)
        {
            xLockPos = position;
            xLock = true;
        }
        else
            xLock = false;
    }

    public void ChangeYLock(bool shouldLock, float position)
    {
        if (shouldLock == yLock)
            return;

        if (shouldLock)
        {
            yLockPos = position;
            yLock = true;
        }
        else
            yLock = false;
    }
}

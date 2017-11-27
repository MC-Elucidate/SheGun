using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorPlatform : MonoBehaviour {

    [SerializeField]
    Vector3 leftPosition;
    [SerializeField]
    Vector3 rightPosition;

    [SerializeField]
    EConveyorState currentState;
    [SerializeField]
    EConveyorRotation currentRotation;

    [SerializeField]
    private float speed = 2;

    private Rigidbody2D platformRigidbody;

    void Start () {
        platformRigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        //platformRigidbody.velocity = new Vector2(speed, 0);
    }

    private enum EConveyorState
    {
        Top,
        Rotating,
        Bottom
    }

    private enum EConveyorRotation
    {
        Clockwise,
        CounterClockwise
    }

    private void Rotate()
    {

    }

    private void MoveLeft()
    { }
}

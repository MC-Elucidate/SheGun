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
	
	void FixedUpdate ()
    {
        //platformRigidbody.velocity = new Vector2(speed, 0);
        platformRigidbody.MovePosition(new Vector2(transform.position.x + (speed * Time.fixedDeltaTime), transform.position.y));
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

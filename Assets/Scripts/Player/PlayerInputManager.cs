using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementManager))]
[RequireComponent(typeof(FireManager))]
public class PlayerInputManager : MonoBehaviour {

    private MovementManager movementManager;
    private FireManager fireManager;
    private MeleeManager meleeManager;
    private GunDatsuManager gunDatsuManager;
    private EDirection directionPressed;

    void Start ()
    {
        movementManager = GetComponent<MovementManager>();
        fireManager = GetComponent<FireManager>();
        meleeManager = GetComponent<MeleeManager>();
        gunDatsuManager = GetComponent<GunDatsuManager>();
    }
	
	void Update ()
    {
        MovementInput();
        FireInput();
        MeleeInput();
        JumpInput();
        GunDatsuInput();
	}

    private void MovementInput()
    {
        float input = Input.GetAxis("Horizontal");
        movementManager.MovementInput = input;
    }

    private void FireInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        if (verticalInput == 1)
        {
            if (horizontalInput == 1)
                directionPressed = EDirection.UpRight;
            else if (horizontalInput == -1)
                directionPressed = EDirection.UpLeft;
            else
                directionPressed = EDirection.Up;
        }
        else if (verticalInput == -1)
        {
            if (horizontalInput == 1)
                directionPressed = EDirection.DownRight;
            else if (horizontalInput == -1)
                directionPressed = EDirection.DownLeft;
            else
                directionPressed = EDirection.Down;
        }
        else
        {
            if (horizontalInput == 1)
                directionPressed = EDirection.Right;
            else if (horizontalInput == -1)
                directionPressed = EDirection.Left;
            else
                directionPressed = EDirection.Neutral;
        }

        fireManager.DirectionInput(directionPressed);
        if (Input.GetButtonDown("Fire"))
            fireManager.FirePressed();
        if (Input.GetButtonUp("Fire"))
            fireManager.FireReleased();
    }

    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump"))
            movementManager.Jump();
    }

    private void MeleeInput()
    {
        if (Input.GetButtonDown("Attack"))
            meleeManager.AttackPressed(directionPressed);
    }

    private void GunDatsuInput()
    {

        if (Input.GetButtonDown("Dodge"))
            gunDatsuManager.DodgePressed();

        if (Input.GetButtonDown("GunDatsu"))
            gunDatsuManager.GunDatsuPressed();
        if (Input.GetButtonUp("GunDatsu"))
            gunDatsuManager.GunDatsuReleased();

        float horInput = Input.GetAxis("GunHorizontal");
        float verInput = Input.GetAxis("GunVertical");
        gunDatsuManager.AimInput(horInput, verInput);
    }
}

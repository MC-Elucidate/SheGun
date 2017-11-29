using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementManager))]
public class FireManager : MonoBehaviour {

    private EDirection direction = EDirection.Neutral;
    private MovementManager movementManager;
    private AudioSource audioSource;
    private GunDatsuManager gunDatsuManager;
    private Animator animator;
    
    private bool reloading = false;

    [SerializeField]
    private GameObject Gunfire;

    [SerializeField]
    private AudioClip shotgunFireSound;

    void Start () {
        movementManager = GetComponent<MovementManager>();
        audioSource = GetComponent<AudioSource>();
        gunDatsuManager = GetComponent<GunDatsuManager>();
        animator = GetComponent<Animator>();
	}
	
	void Update () {
		
	}

    public void DirectionInput(EDirection direction)
    {
        this.direction = direction;
    }

    public void FirePressed()
    {
        if (gunDatsuManager.InGunDatsu)
            FirePistol(gunDatsuManager.GetTargetPosition());
        else
            FirePistol(DirectionHelper.GetDirectionVector(movementManager.forwardDirection));
    }

    private void FireShotgun(Vector3 velocityDirection)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject gunfireInstance = GameObject.Instantiate(Gunfire, transform.position, Quaternion.identity);
            Bullet bullet = gunfireInstance.GetComponent<Bullet>();
            bullet.SetFireDirection(velocityDirection, -6 + i * 3);
        }
        audioSource.PlayOneShot(shotgunFireSound);
        animator.SetTrigger("Fire");
    }

    private void FirePistol(Vector3 velocityDirection)
    {
        GameObject gunfireInstance = GameObject.Instantiate(Gunfire, transform.position, Quaternion.identity);
        Bullet bullet = gunfireInstance.GetComponent<Bullet>();
        bullet.SetFireDirection(velocityDirection, 0);
    }
}

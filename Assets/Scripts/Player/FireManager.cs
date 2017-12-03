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
    private float chargeTimeMax;
    private bool charging = false;
    private float chargeTimeCurrent = 0;

    [SerializeField]
    private GameObject Gunfire;

    [SerializeField]
    private AudioClip shotgunFireSound;
    [SerializeField]
    private AudioClip shotgunChargedSound;

    [SerializeField]
    private int angleBetweenShotgunShells;

    void Start () {
        movementManager = GetComponent<MovementManager>();
        audioSource = GetComponent<AudioSource>();
        gunDatsuManager = GetComponent<GunDatsuManager>();
        animator = GetComponent<Animator>();
	}
	
	void Update () {
        ChargeUpdate();
	}

    private void ChargeUpdate()
    {
        if (charging)
        {
            chargeTimeCurrent += Time.unscaledDeltaTime;

            if (chargeTimeCurrent >= chargeTimeMax)
            {
                charging = false;
                audioSource.PlayOneShot(shotgunChargedSound);
            }
        }
    }

    public void DirectionInput(EDirection direction)
    {
        this.direction = direction;
    }

    public void FirePressed()
    {
        charging = true;
    }

    public void FireReleased()
    {
        if (chargeTimeCurrent >= chargeTimeMax)
        {
            if (gunDatsuManager.InGunDatsu)
                FireShotgun(gunDatsuManager.GetTargetPosition());
            else
                FireShotgun(DirectionHelper.GetDirectionVector(movementManager.forwardDirection));
        }
        else
        {
            if (gunDatsuManager.InGunDatsu)
                FirePistol(gunDatsuManager.GetTargetPosition());
            else
                FirePistol(DirectionHelper.GetDirectionVector(movementManager.forwardDirection));
        }

        charging = false;
        chargeTimeCurrent = 0;
    }

    private void FireShotgun(Vector3 velocityDirection)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject gunfireInstance = GameObject.Instantiate(Gunfire, transform.position, Quaternion.identity);
            Bullet bullet = gunfireInstance.GetComponent<Bullet>();
            bullet.SetFireDirection(velocityDirection, (angleBetweenShotgunShells*-2) + i * angleBetweenShotgunShells);
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

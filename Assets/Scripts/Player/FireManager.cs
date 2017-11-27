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
    
    [SerializeField]
    private float reloadTime = 1.5f;
    [SerializeField]
    private float kickbackPower = 10f;
    
    private bool reloading = false;

    [SerializeField]
    private GameObject Gunfire;

    [SerializeField]
    private AudioClip fireSound;

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
        if (reloading)
            return;

        if (gunDatsuManager.InGunDatsu)
            Fire(gunDatsuManager.GetTargetPosition());
        else
            Fire(DirectionHelper.GetDirectionVector(direction));
    }

    private void Fire(Vector3 velocityDirection)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject gunfireInstance = GameObject.Instantiate(Gunfire, transform.position, Quaternion.identity);
            Bullet bullet = gunfireInstance.GetComponent<Bullet>();
            bullet.SetFireDirection(velocityDirection, -6 + i * 3);
        }
        audioSource.PlayOneShot(fireSound);

        if(!movementManager.IsGrounded)
            movementManager.KickBack(direction, kickbackPower);

        animator.SetTrigger("Fire");
        StartReload();
    }

    public void StartReload()
    {
        if (reloading)
            return;

        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSecondsRealtime(reloadTime);
        reloading = false;
    }
}

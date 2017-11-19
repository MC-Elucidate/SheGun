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

    [SerializeField]
    public int ammoCapacity = 2;
    [SerializeField]
    private float reloadTime = 2f;
    [SerializeField]
    private float firePower = 10f;

    public int currentAmmo;
    private bool reloading = false;

    [SerializeField]
    private GameObject Gunfire;

    [SerializeField]
    private AudioClip fireSound;
    [SerializeField]
    private AudioClip reloadingSound;
    [SerializeField]
    private AudioClip reloadCompleteSound;

    void Start () {
        currentAmmo = ammoCapacity;
        movementManager = GetComponent<MovementManager>();
        audioSource = GetComponent<AudioSource>();
        gunDatsuManager = GetComponent<GunDatsuManager>();
	}
	
	void Update () {
		
	}

    public void DirectionInput(EDirection direction)
    {
        this.direction = direction;
    }

    public void FirePressed()
    {
        if (currentAmmo == 0)
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
        movementManager.KickBack(direction, firePower);
        --currentAmmo;
    }

    public void StartReload()
    {
        if (reloading)
            return;

        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        audioSource.PlayOneShot(reloadingSound);
        reloading = true;
        yield return new WaitForSecondsRealtime(reloadTime);
        currentAmmo = ammoCapacity;
        reloading = false;
        audioSource.PlayOneShot(reloadCompleteSound);
    }
}

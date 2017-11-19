using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementManager))]
public class FireManager : MonoBehaviour {

    private EDirection direction = EDirection.Neutral;
    private MovementManager movementManager;
    private AudioSource audioSource;

    [SerializeField]
    private int ammoCapacity = 2;
    [SerializeField]
    private float reloadTime = 2f;
    [SerializeField]
    private float firePower = 10f;

    private int currentAmmo;
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
	}
	
	void Update () {
		
	}

    public void DirectionInput(EDirection direction)
    {
        this.direction = direction;
    }

    public void Fire()
    {
        if (currentAmmo == 0)
            return;

        for (int i = 0; i < 5; i++)
        {
            GameObject gunfireInstance = GameObject.Instantiate(Gunfire, transform.position, Quaternion.identity);
            Bullet bullet = gunfireInstance.GetComponent<Bullet>();
            bullet.SetFireDirection(direction, -10 + i*5);
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

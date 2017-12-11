using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDatsuManager : MonoBehaviour {

    [SerializeField]
    private float timeSlowRatio = 0.1f;

    [SerializeField]
    public float energyMax = 100;
    [ReadOnly]
    public float energyCurrent;
    [SerializeField]
    private float gunDatsuDrainAmountPerSecond = 20;

    private GunDatsuHitbox gunDatsuHitbox;
    private MovementManager movementManager;
    
    public bool InGunDatsu { get; private set; }

    private Transform enemyToDodge;
    private Vector2 castDirection;
    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private CameraManager cameraManager;

    [SerializeField]
    private GameObject aimLine;
    private LineRenderer aimLineInstance;

    void Start() {
        gunDatsuHitbox = GetComponentInChildren<GunDatsuHitbox>();
        movementManager = GetComponent<MovementManager>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cameraManager = Camera.main.GetComponent<CameraManager>();
        InGunDatsu = false;
        energyCurrent = energyMax;
    }

    void Update() {
        if (InGunDatsu)
            DrainMeter();
    }

    private void DrainMeter()
    {
        energyCurrent -= gunDatsuDrainAmountPerSecond * Time.unscaledDeltaTime;
        if (energyCurrent <= 0)
            EndGunDatsu();
    }

    public void DodgePressed()
    {
        return;
        StartGunDatsu();
        enemyToDodge = gunDatsuHitbox.FindEnemyInDesperationAttackInRange();
        if (enemyToDodge == null)
            return;
        SetCollisionWithEnemies(false);
        playerRigidbody.gravityScale = 0;
        movementManager.DashInDirectionOf(enemyToDodge);
    }

    private void StartGunDatsu()
    {
        
        Time.timeScale = timeSlowRatio;
        aimLineInstance = Instantiate(aimLine).GetComponent<LineRenderer>();
        InGunDatsu = true;
        movementManager.StartGunDatsuMovement();
        cameraManager.ShowTargets();
        animator.SetBool("Dash", InGunDatsu);
    }


    private void EndGunDatsu()
    {
        if (!InGunDatsu)
            return;
        
        Time.timeScale = 1;
        Destroy(aimLineInstance.gameObject);
        InGunDatsu = false;
        energyCurrent = energyMax;
        movementManager.EndGunDatsuMovement();
        cameraManager.HideTargets();
        animator.SetBool("Dash", InGunDatsu);
    }

    private void SetCollisionWithEnemies(bool shouldCollide)
    {
        Physics2D.IgnoreLayerCollision(8, 10, !shouldCollide);
        Physics2D.IgnoreLayerCollision(8, 11, !shouldCollide);
    }

    public void AimInput(float horizontal, float vertical)
    {
        if (!InGunDatsu)
            return;

        castDirection = new Vector2();
        Vector3 target = new Vector3();
        RaycastHit2D hit;

        if (horizontal == 0 && vertical == 0)
        {
            castDirection = DirectionHelper.GetDirectionVector(movementManager.forwardDirection);
        }
        else
        {
            Vector2 playerPos2D = new Vector2(transform.position.x, transform.position.y);
            castDirection = (playerPos2D + new Vector2(horizontal, vertical)) - playerPos2D;
        }
        hit = Physics2D.Raycast(transform.position, castDirection, Mathf.Infinity, LayerMask.GetMask("Enemy"));
        if (hit)
            target = hit.point;
        else
            target = castDirection * 10000;

        aimLineInstance.SetPositions(new Vector3[] { transform.position, target });
    }

    public Vector3 GetTargetPosition()
    {
        if (castDirection != new Vector2())
            return castDirection.normalized;
        else
            return (enemyToDodge.position - transform.position).normalized;
    }

    public void GunDatsuPressed()
    {
        StartGunDatsu();
    }

    public void GunDatsuReleased()
    {
        EndGunDatsu();
    }
}

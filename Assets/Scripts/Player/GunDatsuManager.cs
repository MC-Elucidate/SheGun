using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDatsuManager : MonoBehaviour {

    [SerializeField]
    private float TimeSlowRatio = 0.5f;
    [SerializeField]
    private float TimeSlowTime = 3f;

    private GunDatsuHitbox gunDatsuHitbox;
    private MovementManager movementManager;

    public bool InGunDatsu { get; private set; }
    private Transform enemyToDodge;
    private Vector2 castDirection;
    private Rigidbody2D playerRigidbody;
    private Animator animator;

    [SerializeField]
    private GameObject aimLine;
    private LineRenderer aimLineInstance;

    void Start() {
        gunDatsuHitbox = GetComponentInChildren<GunDatsuHitbox>();
        movementManager = GetComponent<MovementManager>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        InGunDatsu = false;
    }

    void Update() {

    }

    public void DodgePressed()
    {
        StartGunDatsu();
    }

    private void StartGunDatsu()
    {
        enemyToDodge = gunDatsuHitbox.FindEnemyInDesperationAttackInRange();
        if (enemyToDodge == null)
            return;
        SetCollisionWithEnemies(false);
        movementManager.DashInDirectionOf(enemyToDodge);
        Time.timeScale = TimeSlowRatio;
        aimLineInstance = Instantiate(aimLine).GetComponent<LineRenderer>();
        playerRigidbody.gravityScale = 0;
        InGunDatsu = true;
        StartCoroutine(EndGunDatsuTimer());
    }

    private IEnumerator EndGunDatsuTimer()
    {
        yield return new WaitForSecondsRealtime(TimeSlowTime);
        EndGunDatsu();
    }

    private void EndGunDatsu()
    {
        if (!InGunDatsu)
            return;

        SetCollisionWithEnemies(true);
        Time.timeScale = 1;
        playerRigidbody.gravityScale = 1;
        Destroy(aimLineInstance);
        InGunDatsu = false;
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
            target = enemyToDodge.position;
        else
        {
            Vector2 playerPos2D = new Vector2(transform.position.x, transform.position.y);
            castDirection = (playerPos2D + new Vector2(horizontal, vertical)) - playerPos2D;
            hit = Physics2D.Raycast(transform.position, castDirection, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            if (hit)
                target = hit.point;
            else
                target = castDirection * 10000;
           
        }

        aimLineInstance.SetPositions(new Vector3[] { transform.position, target });
    }

    public Vector3 GetTargetPosition()
    {
        EndGunDatsu();

        if (castDirection != new Vector2())
            return castDirection.normalized;
        else
            return (enemyToDodge.position - transform.position).normalized;
    }

    void LateUpdate()
    {
        animator.SetBool("Dash", InGunDatsu);
    }
}

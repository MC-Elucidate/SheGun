using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour {

    [SerializeField]
    private bool firing = false;
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float activeTime;
    private float currentTimeActive = 0;

    public float laserLength;
    public float laserWidth;
    private PlayerStatusManager player;

    private Vector3 beamDestination;

    [SerializeField]
    private GameObject laserBeamLine;
    private LineRenderer laserBeamLineInstance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<PlayerStatusManager>();
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position-new Vector3(0, 100, 0));
        laserLength = hit.distance;
        beamDestination = hit.point;
    }

    void Update ()
    {
        CheckFiringTimer();
        CastLaserDamage();
	}

    private void CheckFiringTimer()
    {
        if (!firing)
            return;
        currentTimeActive += Time.deltaTime;
        if (currentTimeActive >= activeTime)
            TurnLaserOff();
    }

    private void CastLaserDamage()
    {
        if (!firing)
            return;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(laserWidth, 0.1f), 0, Vector2.down, laserLength, LayerMask.GetMask(Constants.Layers.Player));
        Debug.DrawLine(transform.position + new Vector3(-laserWidth/2, 0, 0), transform.position + new Vector3(-laserWidth/2, -laserLength, 0), Color.cyan);
        Debug.DrawLine(transform.position + new Vector3(laserWidth/2, 0, 0), transform.position + new Vector3(laserWidth/2, -laserLength, 0), Color.cyan);

        if (hit)
            player.ReceiveDamage(damage, transform);
    }

    public void TurnLaserOn()
    {
        firing = true;
        laserBeamLineInstance = Instantiate(laserBeamLine).GetComponent<LineRenderer>();
        laserBeamLineInstance.transform.parent = transform;
        laserBeamLineInstance.SetPositions(new Vector3[] { transform.position, beamDestination });
    }

    private void TurnLaserOff()
    {
        firing = false;
        Destroy(laserBeamLineInstance.gameObject);
        currentTimeActive = 0;
    }


}
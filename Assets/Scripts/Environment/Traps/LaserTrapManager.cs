using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrapManager : MonoBehaviour {

    private LaserTrap[] managedLasers;

    [SerializeField]
    private float timeBetweenBeams;
    private float timeSinceLastBeam = 0;
    private int beamToFire = 0;

	void Start () {
        managedLasers = GetComponentsInChildren<LaserTrap>();
        timeSinceLastBeam = timeBetweenBeams;
	}
	
	void Update () {
        CheckFireTimer();
	}

    private void CheckFireTimer()
    {
        timeSinceLastBeam += Time.deltaTime;
        if (timeSinceLastBeam > timeBetweenBeams)
            FireNextLaser();
    }

    private void FireNextLaser()
    {
        managedLasers[beamToFire].TurnLaserOn();
        beamToFire++;
        timeSinceLastBeam = 0;
        if (beamToFire >= managedLasers.Length)
            beamToFire = 0;
    }
}

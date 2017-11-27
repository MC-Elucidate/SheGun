using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingSpawner : MonoBehaviour {

    [SerializeField]
    private float spawnInterval;
    private float timeSinceSpawn = 0;

    [SerializeField]
    private GameObject prefabToSpawn;

	void Start () {
		
	}
	
	void Update () {
        if (timeSinceSpawn >= spawnInterval)
            SpawnPrefab();
        else
            timeSinceSpawn += Time.deltaTime;
	}

    private void SpawnPrefab()
    {
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        timeSinceSpawn = 0;
    }
}

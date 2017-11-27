using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {

    [SerializeField]
    private float aliveTime = 0;

	void Start () {
        Destroy(gameObject, aliveTime);
	}
}

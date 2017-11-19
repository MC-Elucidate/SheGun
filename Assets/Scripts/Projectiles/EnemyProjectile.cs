using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour {

    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private int damage = 1;

    public Vector3 TargetPosition;
    private Rigidbody2D projectileRigidbody;

	void Start () {
        projectileRigidbody = GetComponent<Rigidbody2D>();
        projectileRigidbody.velocity = (TargetPosition - transform.position).normalized * speed;
        projectileRigidbody.gravityScale = 0;
        Destroy(gameObject, 3f);
	}
	
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStatusManager>().ReceiveDamage(damage);
        }
        Destroy(gameObject);
    }
}

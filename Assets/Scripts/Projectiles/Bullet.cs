using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

    private Rigidbody2D bulletRigidbody;
    private Vector2 fireDirection;
    private float rotationOffset;

    [SerializeField]
    private float lifespan = 2f;
    [SerializeField]
    private float speed = 15f;
    [SerializeField]
    private int damage = 1;

    void Start () {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.gravityScale = 0;
        bulletRigidbody.velocity = (Quaternion.AngleAxis(rotationOffset, Vector3.forward) * fireDirection) * speed;
        Destroy(gameObject, lifespan);
    }
	
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Constants.Tags.Enemy)
            collision.gameObject.GetComponent<EnemyStatusManager>().ReceiveBulletDamage(damage);

        Destroy(gameObject);
    }

    public void SetFireDirection(Vector2 direction, float rotationOffset)
    {
        fireDirection = direction;
        this.rotationOffset = rotationOffset;
    }
}

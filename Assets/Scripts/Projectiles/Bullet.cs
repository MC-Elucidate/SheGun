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
    [SerializeField]
    private bool gunDatsuProjectile;

    void Start () {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.gravityScale = 0;
        bulletRigidbody.velocity = (Quaternion.AngleAxis(rotationOffset, Vector3.forward) * fireDirection) * speed;
        Destroy(gameObject, lifespan);
    }
	
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Constants.Tags.Enemy)
            other.gameObject.GetComponent<EnemyStatusManager>().ReceiveBulletDamage(damage);

        if(!gunDatsuProjectile)
            Destroy(gameObject);
    }

    public void SetFireDirection(Vector2 direction, float rotationOffset)
    {
        fireDirection = direction;
        this.rotationOffset = rotationOffset;
    }
}

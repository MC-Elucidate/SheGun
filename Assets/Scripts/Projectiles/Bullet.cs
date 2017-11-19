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
        if (collision.gameObject.tag == "Enemy")
            collision.gameObject.GetComponent<EnemyStatusManager>().ReceiveBulletDamage(damage);

        Destroy(gameObject);
    }

    public void SetFireDirection(EDirection direction, float rotationOffset)
    {
        fireDirection = new Vector2();
        this.rotationOffset = rotationOffset;

        if (direction == EDirection.Neutral)
        {
            fireDirection = new Vector2(1, 0);
            return;
        }

        switch (direction)
        {
            case EDirection.Down:
            case EDirection.DownLeft:
            case EDirection.DownRight:
                fireDirection.y = -1;
                break;
            case EDirection.Up:
            case EDirection.UpLeft:
            case EDirection.UpRight:
                fireDirection.y = 1;
                break;
        }
        switch (direction)
        {
            case EDirection.Left:
            case EDirection.DownLeft:
            case EDirection.UpLeft:
                fireDirection.x = -1;
                break;
            case EDirection.Right:
            case EDirection.DownRight:
            case EDirection.UpRight:
                fireDirection.x = 1;
                break;
        }
    }
}

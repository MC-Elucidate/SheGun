using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyStatusManager : MonoBehaviour {

    [SerializeField]
    protected int Health;
    [SerializeField]
    protected int executableThreshold;

    protected GameObject player;
    protected PlayerStatusManager playerStatus;
    protected Rigidbody2D enemyRigidbody;
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected Color defaultColour;
    private Color damageFlashColour;
    protected EDirection forwardDirection = EDirection.Left;

    protected virtual void Start () {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultColour = sprite.color;
        damageFlashColour = Color.red;
        player = GameObject.FindGameObjectWithTag("Player");
        playerStatus = player.GetComponent<PlayerStatusManager>();
	}
	
	protected virtual void Update () {
        SetSpriteDirection();
    }

    public virtual bool ReceiveBulletDamage(int damage)
    {
        TakeDamage(damage);
        return true;
    }

    public virtual void ReceiveMeleeDamage(int damage)
    {
        if (damage > 0)
            TakeDamage(damage);
    }

    private void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            Destroy(gameObject, 0.5f);

        if (Health < executableThreshold)
            defaultColour = Color.green;
        StartCoroutine(DamageFlash());

    }

    private void Launched()
    {
        enemyRigidbody.velocity = new Vector2(enemyRigidbody.velocity.x, 10);
    }

    protected IEnumerator DamageFlash()
    {
        sprite.color = damageFlashColour;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.color = defaultColour;
    }

    public virtual bool IsExecutable()
    {
        return Health < executableThreshold;
    }



    private void SetSpriteDirection()
    {
        if (player.transform.position.x < transform.position.x && forwardDirection == EDirection.Right)
        {
            FlipSprite();
            forwardDirection = EDirection.Left;
        }
        else if (player.transform.position.x > transform.position.x && forwardDirection == EDirection.Left)
        {
            forwardDirection = EDirection.Right;
            FlipSprite();
        }

    }

    private void FlipSprite()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }

    public abstract void CollideWithPlayer();
}

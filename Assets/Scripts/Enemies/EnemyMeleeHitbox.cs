using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour {
    
    private Collider2D hitbox;
    public PlayerStatusManager playerStatus;
    int damageOfAttack;

    void Start()
    {
        hitbox = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            playerStatus.ReceiveDamage(damageOfAttack);
    }

    public void BeginAttack(int damage)
    {
        damageOfAttack = damage;
        hitbox.enabled = true;
    }

    public void EndAttack()
    {
        hitbox.enabled = false;
    }
}

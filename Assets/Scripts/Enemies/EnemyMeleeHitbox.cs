using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour {

    bool playerInRange = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            playerInRange = false;
    }

    public void AttackCast(PlayerStatusManager playerStatus, int damage)
    {
        if(playerInRange)
            playerStatus.ReceiveDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MeleeHitbox : MonoBehaviour {

    private List<EnemyStatusManager> EnemiesInRange;

    void Start()
    {
        EnemiesInRange = new List<EnemyStatusManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Constants.Tags.Enemy)
            EnemiesInRange.Add(other.GetComponent<EnemyStatusManager>());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Constants.Tags.Enemy)
            EnemiesInRange.Remove(other.GetComponent<EnemyStatusManager>());
    }

    public void AttackEnemies(bool launchingAttack, int damage)
    {
        List<EnemyStatusManager> nullEnemies = new List<EnemyStatusManager>();
        foreach (EnemyStatusManager enemy in EnemiesInRange)
        {
            if (enemy == null)
            {
                nullEnemies.Add(enemy);
                continue;
            }
            else
                enemy.ReceiveMeleeDamage(launchingAttack, damage);
        }
            RemoveNullEnemies(nullEnemies);
    }

    private void RemoveNullEnemies(List<EnemyStatusManager> nullEnemies)
    {
        foreach (EnemyStatusManager deadEnemy in nullEnemies)
            EnemiesInRange.Remove(deadEnemy);
    }

}

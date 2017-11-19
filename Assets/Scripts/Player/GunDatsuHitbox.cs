using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDatsuHitbox : MonoBehaviour {

    private List<EnemyStatusManager> EnemiesInRange;

    void Start()
    {
        EnemiesInRange = new List<EnemyStatusManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
            EnemiesInRange.Add(other.GetComponent<EnemyStatusManager>());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
            EnemiesInRange.Remove(other.GetComponent<EnemyStatusManager>());
    }

    public Transform FindEnemyInDesperationAttackInRange()
    {
        List<EnemyStatusManager> nullEnemies = new List<EnemyStatusManager>();
        EnemyStatusManager firstEnemy = null;
        foreach (EnemyStatusManager enemy in EnemiesInRange)
        {
            if (enemy == null)
            {
                nullEnemies.Add(enemy);
                continue;
            }
            else
            {
                if (firstEnemy == null && enemy.PerformingDesperationAttack())
                    firstEnemy = enemy;
            }
            
            continue;
        }

        RemoveNullEnemies(nullEnemies);

        if (firstEnemy == null)
            return null;

        return firstEnemy.transform;
    }

    private void RemoveNullEnemies(List<EnemyStatusManager> nullEnemies)
    {
        foreach (EnemyStatusManager deadEnemy in nullEnemies)
            EnemiesInRange.Remove(deadEnemy);
    }
}

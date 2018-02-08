using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Constants.Tags.Player)
        {
            other.gameObject.GetComponent<PlayerStatusManager>().UpgradeHealth();
            Destroy(gameObject);
        }
    }
}

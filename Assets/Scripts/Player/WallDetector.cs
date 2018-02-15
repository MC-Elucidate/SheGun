using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour {

    [ReadOnly]
    public bool IsTouchingWall = false;
    [ReadOnly]
    public EDirection wallDirection = EDirection.Neutral;
    
	void OnTriggerEnter2D(Collider2D wall)
    {
        IsTouchingWall = true;
        wallDirection = wall.transform.position.x - transform.position.x > 0 ? EDirection.Right : EDirection.Left;
    }

    void OnTriggerExit2D()
    {
        IsTouchingWall = false;
        wallDirection = EDirection.Neutral;
    }
}

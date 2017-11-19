using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionHelper{

    public static Vector2 GetDirectionVector(EDirection direction)
    {
        if (direction == EDirection.Neutral)
        {
            return new Vector2(1, 0);
        }

        Vector2 directionVector = new Vector2();

        switch (direction)
        {
            case EDirection.Down:
            case EDirection.DownLeft:
            case EDirection.DownRight:
                directionVector.y = -1;
                break;
            case EDirection.Up:
            case EDirection.UpLeft:
            case EDirection.UpRight:
                directionVector.y = 1;
                break;
        }
        switch (direction)
        {
            case EDirection.Left:
            case EDirection.DownLeft:
            case EDirection.UpLeft:
                directionVector.x = -1;
                break;
            case EDirection.Right:
            case EDirection.DownRight:
            case EDirection.UpRight:
                directionVector.x = 1;
                break;
        }

        return directionVector.normalized;
    }
}

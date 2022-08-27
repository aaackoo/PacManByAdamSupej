using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWaypointsController : MonoBehaviour
{
    public GameObject leftWaypoint;
    public GameObject upWaypoint;
    public GameObject rightWaypoint;
    public GameObject downWaypoint;

    public bool canMoveLeft;
    public bool canMoveUp;
    public bool canMoveRight;
    public bool canMoveDown;

    public bool canMove(Vector2 direction)
    {
        if (direction == Vector2.left)
            return canMoveLeft;
        if (direction == Vector2.up)
            return canMoveUp;
        if (direction == Vector2.right)
            return canMoveRight;
        if (direction == Vector2.down)
            return canMoveDown;
        return false;
    }
}

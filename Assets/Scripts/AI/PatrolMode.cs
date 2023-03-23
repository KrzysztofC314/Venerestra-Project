using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMode : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";
    private AIController controller;

    [SerializeField]
    Transform linecastPosition;
    [SerializeField]
    Transform boxcastPosition;
    [SerializeField]
    float baseCastDistance;
    [SerializeField]
    Vector2 baseBoxcastSize;

    private string facingDirection;

    [SerializeField]
    private float patrolSpeed;
    [SerializeField]
    private float ledgeWait;


    void Start()
    {
        controller = this.GetComponent<AIController>();
        controller.speed = patrolSpeed;
    }

    public void FixedUpdate()
    {
        facingDirection = controller.facingDirection;

        
        if (IsHittingWall()||IsNearEdge())
        {
            if (facingDirection == Left)
            {
                controller.speed = 0;
                Invoke("TurnRight", ledgeWait);
            }
            else if (facingDirection == Right)
            {
                controller.speed = 0;
                Invoke("TurnLeft", ledgeWait);
            }
        }
    }

    private void TurnLeft()
    {
        controller.ChangeDirection(Left);
        controller.speed = patrolSpeed;
    }

    private void TurnRight()
    {
        controller.ChangeDirection(Right);
        controller.speed = patrolSpeed;
    }

    bool IsHittingWall()
    {
        bool val = false;

        if(Physics2D.BoxCast(boxcastPosition.position,baseBoxcastSize,0f,Vector2.zero,LayerMask.NameToLayer("Terrain")))
        {
            val = true;
        }

        return val;
    }

    bool IsNearEdge()
    {
        bool val = true;
        float castDistance = baseCastDistance;

        Vector3 targetPosition = linecastPosition.position;
        targetPosition.y -= castDistance;

        if (Physics2D.Linecast(linecastPosition.position, targetPosition, 1 << LayerMask.NameToLayer("Terrain")))
        {
            val = false;
        }

        return val;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(boxcastPosition.position, new Vector3(baseBoxcastSize.x, baseBoxcastSize.y, 1));
    }
}

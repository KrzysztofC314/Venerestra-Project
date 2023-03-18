using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMode : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";
    private AIController controller;

    [SerializeField]
    Transform castPosition;
    [SerializeField]
    float baseCastDistance;

    private string facingDirection;

    Rigidbody2D rb;
    [SerializeField]
    float speed = 5;

    void Start()
    {
        controller = this.GetComponent<AIController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        facingDirection = controller.facingDirection;
        float velocityX = speed;

        if(facingDirection == Left)
        {
            velocityX = -speed;
        }

        rb.velocity = new Vector2(velocityX, rb.velocity.y);
        if (IsHittingWall()||IsNearEdge())
        {
            if (facingDirection == Left)
            {
                controller.ChangeDirection(Right);
            }
            else if (facingDirection == Right)
            {
                controller.ChangeDirection(Left);
            }
        }
    }

    bool IsHittingWall()
    {
        bool val = false;
        float castDistance = baseCastDistance;
        if (facingDirection == Left)
        {
            castDistance = -baseCastDistance;
        }

        Vector3 targetPosition = castPosition.position;
        targetPosition.x += castDistance;

        if(Physics2D.Linecast(castPosition.position, targetPosition, 1 << LayerMask.NameToLayer("Terrain")))
        {
            val = true;
        }

        return val;
    }

    bool IsNearEdge()
    {
        bool val = true;
        float castDistance = baseCastDistance;

        Vector3 targetPosition = castPosition.position;
        targetPosition.y -= castDistance;

        if (Physics2D.Linecast(castPosition.position, targetPosition, 1 << LayerMask.NameToLayer("Terrain")))
        {
            val = false;
        }

        return val;
    }
}

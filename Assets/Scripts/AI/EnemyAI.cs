using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";

    [SerializeField]
    Transform castPosition;
    [SerializeField]
    float baseCastDistance;

    [SerializeField]
    Transform visionPosition;
    [SerializeField]
    float baseVisionCone;
    [SerializeField]
    float baseVisionRange;

    Vector3 baseScale;

    string facingDirection;

    Rigidbody2D rigidbody;
    [SerializeField]
    float speed = 5;

    void Start()
    {
        facingDirection = Right;
        baseScale = transform.localScale;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float velocityX = speed;

        if(facingDirection == Left)
        {
            velocityX = -speed;
        }

        rigidbody.velocity = new Vector2(velocityX, rigidbody.velocity.y);
        if (IsHittingWall()||IsNearEdge())
        {
            if (facingDirection == Left)
            {
                ChangeDirection(Right);
            }
            else if (facingDirection == Right)
            {
                ChangeDirection(Left);
            }
        }
    }

    void ChangeDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if (newDirection == Left)
        {
            newScale.x = -baseScale.x;
        }
        else if (newDirection == Right)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;

        facingDirection = newDirection;
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

    bool DoesSeeEnemy()
    {
        bool val = false;
        float visionRange = baseVisionRange;
        if (facingDirection == Left)
        {
            visionRange = -baseVisionRange;
        }
        return val;
        float angle = Vector3.Angle(baseScale, visionPosition.right);
        RaycastHit2D ray = Physics2D.Raycast(visionPosition.position, baseScale, visionRange);
    }
}

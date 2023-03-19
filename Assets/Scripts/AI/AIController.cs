using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";

    private Rigidbody2D rb;

    [HideInInspector]
    public string facingDirection;
    [HideInInspector]
    public Vector3 baseScale;
    public enum State
    {
        Patrol,
        Chase,
    }

    private PatrolMode patrol;
    private ChaseMode chase;

    public State state;

    public float speed;

    private void Awake()
    {
        state = State.Patrol;
        
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        facingDirection = Right;
        baseScale = transform.localScale;
        patrol = this.GetComponent<PatrolMode>();
        chase = this.GetComponent<ChaseMode>();
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.Patrol:
                patrol.enabled = true;
                chase.enabled = false;
                break;
            case State.Chase:
                patrol.enabled = false;
                chase.enabled = true;
                break;

        }
    }

    private void FixedUpdate()
    {
        float velocityX = speed;

        if (facingDirection == Left)
        {
            velocityX = -speed;
        }

        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    public void ChangeDirection(string newDirection)
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
}

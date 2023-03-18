using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChaseMode : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";
    private AIController controller;
    public Transform target;
    private float timer;
    public float wait;
    public float speed;
    private string facingDirection;
    private float playerPosition;
    private float enemyPosition;
    private float wherePlayer;

    Rigidbody2D rb;

    void Start()
    {
        controller = this.GetComponent<AIController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        playerPosition = target.position.x;
        enemyPosition = this.transform.position.x;
        wherePlayer = playerPosition - enemyPosition;
        facingDirection = controller.facingDirection;
        float velocityX = speed;
        if (facingDirection == Left)
        {
            velocityX = -speed;
        }

        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        if (wherePlayer < 0 && facingDirection == Right)
        {
            controller.ChangeDirection(Left);
        }
        else if (wherePlayer > 0 && facingDirection == Left)
        {
            controller.ChangeDirection(Right);
        }
    }
}

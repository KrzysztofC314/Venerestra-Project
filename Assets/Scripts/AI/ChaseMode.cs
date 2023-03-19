using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChaseMode : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";
    private AIController controller;
    public Transform target;
    [SerializeField]
    private float chaseSpeed;
    [SerializeField]
    private float confusionWait;
    private string facingDirection;
    private float playerPosition;
    private float enemyPosition;
    private float wherePlayer;

    

    void Start()
    {
        controller = this.GetComponent<AIController>();
        controller.speed = chaseSpeed;
    }

    private void FixedUpdate()
    {
        playerPosition = target.position.x;
        enemyPosition = this.transform.position.x;
        wherePlayer = playerPosition - enemyPosition;
        facingDirection = controller.facingDirection;

        if (wherePlayer < 0 && facingDirection == Right)
        {
            controller.speed = 0;
            Invoke("TurnLeft", confusionWait);
        }
        else if (wherePlayer > 0 && facingDirection == Left)
        {
            controller.speed = 0;
            Invoke("TurnRight", confusionWait);
        }

    }
    private void TurnLeft()
    {
        controller.ChangeDirection(Left);
        controller.speed = chaseSpeed;
    }

    private void TurnRight()
    {
        controller.ChangeDirection(Right);
        controller.speed = chaseSpeed;
    }
}

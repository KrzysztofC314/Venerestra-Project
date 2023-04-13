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

    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletTrail;
    [SerializeField] private float weaponRange = 10f;
    [SerializeField] private int weaponDamage = 10;

    

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

    public void Shoot()
    {
        var hit = Physics2D.Raycast(gunPoint.position, transform.right, weaponRange);

        var trail = Instantiate(bulletTrail, gunPoint.position, transform.rotation);

        var trailScript = trail.GetComponent<BulletTrail>();

        if (hit.collider != null)
        {
            trailScript.SetTargetPosition(hit.point);
            var health = hit.collider.GetComponent<PlayerHealth>();
            health.Hurt(weaponDamage);
        }
        else
        {
            var endPostion = gunPoint.position + transform.right * weaponRange;
            trailScript.SetTargetPosition(endPostion);
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

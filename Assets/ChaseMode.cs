using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class ChaseMode : MonoBehaviour
{
    private AIController controller;
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private string facingDirection;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    private float playerPosition;
    private float enemyPosition;
    private float wherePlayer;

    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        controller = this.GetComponent<AIController>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        playerPosition = target.position.x;
        enemyPosition = this.transform.position.x;
        wherePlayer = playerPosition - enemyPosition;
        if (path == null)
            return;

        if (wherePlayer < 0)
        {
            
        }
        else if (wherePlayer > 0)
        {

        }
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}

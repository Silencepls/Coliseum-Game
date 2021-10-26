using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathFinding : MonoBehaviour
{
    public UpdateStats updateStats { get; private set; }
    Rigidbody2D rb;
    Seeker seeker;
    Path path;

    public Transform enemyGFX;
    public Vector2 walkPoint;

    [SerializeField] private int currentWayPoint;
    public float nextWayPointDistance = 1f;
    public bool reachedEndOfPath;

    public bool canMove = true;
    public bool isPatrolling = true;

    private void Start()
    {
        updateStats = GetComponent<UpdateStats>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        // this generates a path, so if you use a coroutine that checks if you can move to activate this function, than it might works.
        InvokeRepeating(nameof(UpdatePath), 0f, .5f);
    }
    void UpdatePath()
    {
        if (seeker.IsDone() && canMove)
        {
            seeker.StartPath((Vector2)transform.position, walkPoint, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    private void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - (Vector2)transform.position).normalized;
        Vector2 force = direction * (updateStats.speed * 1000) * Time.deltaTime;
        if (canMove)
        {
            rb.AddForce(force);
        }

        float distance = Vector2.Distance((Vector2)transform.position, path.vectorPath[currentWayPoint]);
        if(distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        if (isPatrolling)
        {
            if (force.x >= 0.01f)
            {
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (force.x <= -0.01f)
            {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }
    public void StopPath()
    {
        seeker.StartPath((Vector2)transform.position, (Vector2)transform.position, OnPathComplete);
    }
}

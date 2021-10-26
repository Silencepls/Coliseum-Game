using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    protected UpdateStats updateStats;
    protected EnemyPathFinding enemyPathFinding;
    protected Animator animator;

    [SerializeField] protected Vector2 direction;

    [SerializeField] protected bool playerInSightRange;
    [SerializeField] protected bool playerInAttackRange;

    protected Collider2D overlapColliderhit;

    protected bool changeWalkPoint = true;

    public float arenaHeigth = 4;
    public float arenaWidth = 9;

    [SerializeField] protected State state;
    protected enum State
    {
        Patrol,
        Chase,
        Attack
    }

    private void Start()
    {
        updateStats = GetComponent<UpdateStats>();
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        animator = enemyPathFinding.enemyGFX.GetComponent<Animator>();

        state = State.Patrol;
    }
    protected virtual void Update()
    {
        OverlapCircle();
        Animation();

        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }
    protected virtual void Animation()
    {
        if (!enemyPathFinding.canMove)
        {
            animator.SetBool("isWalking", false);
        }
        if (enemyPathFinding.canMove)
        {
            animator.SetBool("isWalking", true);
        }
    }
    protected virtual void OverlapCircle()
    {
        overlapColliderhit = Physics2D.OverlapCircle(transform.position, updateStats.sightRange, LayerMask.GetMask("Player"));
        if (overlapColliderhit != null)
        {
            playerInSightRange = true;
        }
        else
        {
            playerInSightRange = false;
        }
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, updateStats.attackRange, LayerMask.GetMask("Player"));
    }
    protected virtual void Patrol()
    {
        enemyPathFinding.isPatrolling = true;

        if (transform.position.x > arenaWidth || transform.position.y > arenaHeigth)
        {
            changeWalkPoint = true;
        }
        if (changeWalkPoint)
        {
            changeWalkPoint = false;

            float x = Random.Range(-updateStats.walkPointRangeX, updateStats.walkPointRangeX);
            float y = Random.Range(-updateStats.walkPointRangeY, updateStats.walkPointRangeY);
            Vector2 xy = new Vector2(x, y);
            direction = xy + (Vector2)transform.position;
            enemyPathFinding.walkPoint = direction;
        }
        float distance = Vector2.Distance((Vector2)transform.position, direction);

        if (distance < 1f)
        {
            changeWalkPoint = true;
        }

        if (playerInSightRange)
        {
            changeWalkPoint = true;
            state = State.Chase;
        }
    }
    protected virtual void Chase()
    {
        LookingAtTarget();

        if (!playerInSightRange)
        {
            changeWalkPoint = true;
            state = State.Patrol;
            return;
        }
        if (playerInAttackRange)
        {
            state = State.Attack;
        }
        enemyPathFinding.walkPoint = (Vector2)overlapColliderhit.transform.position;
    }
    protected virtual void Attack()
    {
        if (!playerInAttackRange)
        {
            enemyPathFinding.canMove = true;
            state = State.Chase;
            return;
        }
        enemyPathFinding.canMove = false;
        UpdateStats playerStats = overlapColliderhit.GetComponent<UpdateStats>();
        playerStats.TakeDamage(0.01f);

        LookingAtTarget();
    }
    protected virtual void LookingAtTarget()
    {
        enemyPathFinding.isPatrolling = false;

        if (overlapColliderhit.transform.position.x > transform.position.x)
        {
            enemyPathFinding.enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (overlapColliderhit.transform.position.x < transform.position.x)
        {
            enemyPathFinding.enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }

    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody2D playerRigigidbody2D = collision.collider.GetComponent<Rigidbody2D>();
            UpdateStats playerUpdateStats = collision.collider.GetComponent<UpdateStats>();
            Transform playerTransform = collision.collider.transform;

            StartCoroutine(GameManager.instance.SetBurn(playerUpdateStats, 1));
            StartCoroutine(GameManager.instance.KnockBack(playerUpdateStats, playerTransform.position, transform.position, 50, 1));
            StartCoroutine(GameManager.instance.KnockBack(updateStats, transform.position, playerTransform.position, 20, 1));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (updateStats != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, updateStats.walkPointRangeX);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, updateStats.sightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, updateStats.attackRange);
        }
    }
}
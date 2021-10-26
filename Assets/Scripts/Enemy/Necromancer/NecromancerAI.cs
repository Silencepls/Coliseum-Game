using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAI : EnemyAI
{
    protected override void Attack()
    {
        if (!playerInAttackRange)
        {
            enemyPathFinding.canMove = true;
            state = State.Chase;
            return;
        }
        LookingAtTarget();
        Shoot();

        Transform target = overlapColliderhit.transform;
        Vector2 direction = (Vector2)(transform.position - target.position);
        float distance = Vector2.Distance(transform.position, target.position);

        enemyPathFinding.walkPoint = (Vector2)transform.position + direction;
        if(distance >= enemyPathFinding.updateStats.attackRange - 0.5f)
        {
            enemyPathFinding.StopPath();
            enemyPathFinding.canMove = false;
        }
        else
        {
            enemyPathFinding.canMove = true;
        }
    }
    protected override void Chase()
    {
        base.Chase();
        Shoot();
    }
    void Shoot()
    {
        GameManager.instance.SetShoot(enemyPathFinding.updateStats, transform, overlapColliderhit.transform, false);
    }
}

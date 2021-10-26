using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDemon : EnemyAI
{
    [SerializeField] Transform[] ShootPoints;
    protected override void Update()
    {
        base.Update();
        if(GameManager.instance.enemyList.Count > 3)
        {
            enemyPathFinding.updateStats.attackSpeed = 0.1f;
        }
    }
    protected override void Attack()
    {
        base.Attack();
        Shoot();
    }
    protected override void Chase()
    {
        base.Chase();
        Shoot();
    }
    void Shoot()
    {
        GameManager.instance.TransformShoot(enemyPathFinding.updateStats, null, ShootPoints, false, true);
    }
}

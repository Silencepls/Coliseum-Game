using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SwordBasicAttack : MonoBehaviour
{
    Animator swordAnimator;
    [SerializeField] float attackRange = 5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] UpdateStats player;
    [SerializeField] Transform shootingPoint;
    private void Awake()
    {
        swordAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        swordAnimator.SetFloat("attackSpeed", player.attackSpeed);
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            AnimateFirstAttack();
        }
        else
        {
            swordAnimator.SetBool("willAttack", false);
        }
    }
    void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        foreach(Collider2D e in enemies)
        {
            UpdateStats stats = e.GetComponent<UpdateStats>();
            stats.TakeDamage(1);
            StartCoroutine(GameManager.instance.KnockBack(stats, (Vector2)stats.transform.position, (Vector2)player.transform.position, 2.5f, 1));
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public void AnimateFirstAttack()
    {
        swordAnimator.SetBool("willAttack", true);
    }
    public void Shoot()
    {
        GameManager.instance.TransformShoot(player, shootingPoint, null, true, false);
    }
    public void EndAnimation()
    {
        //swordAnimator.SetBool("willAttack", false);
    }
}

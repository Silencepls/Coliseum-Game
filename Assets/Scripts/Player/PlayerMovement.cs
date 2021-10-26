using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    UpdateStats updateStats;

    public bool goldenArmor = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        updateStats = GetComponent<UpdateStats>();
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && goldenArmor)
        {
            UpdateStats updateStatsEnemy = collision.collider.GetComponent<UpdateStats>();
            StartCoroutine(GameManager.instance.SetStun(updateStatsEnemy, 5));
        }
    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        if(direction.sqrMagnitude != 0)
        {
            rb.MovePosition(rb.position + direction * updateStats.speed * Time.fixedDeltaTime);
            //rb.velocity = direction * updateStats.speed * Time.fixedDeltaTime;

            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (horizontal >= 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal <= -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}

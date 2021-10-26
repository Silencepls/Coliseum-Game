using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpike : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Collider2D _collider2D;

    [SerializeField] [Range(0, 1f)] float lerpTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<BoxCollider2D>();
        _collider2D.enabled = false;
        StartCoroutine(ProcedualTimeTrigger());
    }
    IEnumerator ProcedualTimeTrigger()
    {
        while (true)
        {
            int number = Random.Range(0, 12 - GameManager.instance.currentWave);
            if (number == 0)
            {
                ChangeColor();
                yield return new WaitForSeconds(1f);
                spriteRenderer.color = Color.white;
                animator.SetBool("isTriggered", true);
                _collider2D.enabled = true;
                yield return new WaitForSeconds(0.5f);
                animator.SetBool("isTriggered", false);
                animator.SetBool("returnState", true);
                _collider2D.enabled = false;
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
            }
            yield return new WaitForSeconds(2);
            animator.SetBool("returnState", false);
        }
    }
    void ChangeColor()
    {
        spriteRenderer.color = Color.Lerp(Color.white, Color.red, lerpTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UpdateStats updateStats = collision.GetComponent<UpdateStats>();
        if(updateStats != null && collision.CompareTag("Player"))
        {
            updateStats.TakeDamage(9 + GameManager.instance.currentWave);
        }
    }
}

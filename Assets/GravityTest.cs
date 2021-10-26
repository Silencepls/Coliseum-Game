using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTest : MonoBehaviour
{
    Coroutine coroutine = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            coroutine = StartCoroutine(GameManager.instance.Gravity(rb, transform.position, 16));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            StopCoroutine(coroutine);
        }
    }
}

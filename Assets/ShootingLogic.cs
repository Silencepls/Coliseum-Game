using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLogic : MonoBehaviour
{
    private string target { get; set; }
    float damage = 1;

    public bool fireRing = false;

    public void SetTarget(UpdateStats updateStats, bool isEnemyTag)
    {
        if (isEnemyTag)
        {
            target = "Enemy";
        }
        else
        {
            target = "Player";
        }
        damage = updateStats.ad;
        fireRing = updateStats.fireRing;
    }
    private void Update()
    {
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            if(target == "Enemy" && fireRing)
            {
                UpdateStats updateStats = collision.gameObject.GetComponent<UpdateStats>();
                updateStats.TakeDamage(damage);
                StartCoroutine(GameManager.instance.SetBurn(updateStats, 1));
                Destroy(gameObject);
            }
            else
            {
                UpdateStats updateStats = collision.gameObject.GetComponent<UpdateStats>();
                updateStats.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

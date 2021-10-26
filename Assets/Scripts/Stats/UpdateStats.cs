using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateStats : MonoBehaviour
{
    #region variables

    public Stats stats;

    // Current Stats:
    [HideInInspector] public float currentSpeed;
    public float currentAttackSpeed; //

    // Stats:
    [HideInInspector] public float Hp;
    public float currentHp;

    public float speed = 4;
    public float attackSpeed;
    public float attackRange;
    public float ap;
    public float ad;
    public float energy;

    public float armor;

    // Enemies
    public float sightRange;
    public float walkPointRangeX;
    public float walkPointRangeY;

    // Attack
    public bool canAttack = true;

    // Effects
    public float burnDuration = 0;
    public float stunDuration = 0;

    public bool fireRing = false;
    public bool heallingRing = false;

    [SerializeField] SpriteRenderer spriteRenderer;
    public Slider slider;

    [Flags]
    public enum State 
    { 
        Null = 0,
        Stunned = 1,
        Slowed = 2,
        Burning = 4
    }
    public State state;

    #endregion
    private void Awake()
    {
        UpdatingStats();
        currentHp = Hp;
        currentAttackSpeed = attackSpeed;
        state = State.Null;

        if (slider != null)
        {
            slider.maxValue = Hp;
        }
        if (stats.walkPointRangeY == 0)
        {
            stats.walkPointRangeY = stats.walkPointRangeX;
        }
    }
    private void Update()
    {
        if (heallingRing)
        {
            TakeDamage(-0.005f);
        }
        if(currentHp > Hp)
        {
            currentHp = Hp;
        }
        if (slider != null)
        {
            slider.value = currentHp;
        }
        if (currentHp <= 0 && gameObject.CompareTag("Player"))
        {
            TimeManager.instance.StartScene();
        }
        if(currentHp <= 0 && gameObject.CompareTag("Enemy"))
        {
            if (GameManager.instance.enemyList.Contains(gameObject))
            {
                GameManager.instance.enemyList.Remove(gameObject);
            }
            DropChance();
            Destroy(gameObject);
        }

        if (speed != 0 && !state.HasFlag(State.Stunned) && !state.HasFlag(State.Slowed))
        {
            currentSpeed = speed;
        }
    }
    void DropChance()
    {
        int number = UnityEngine.Random.Range(0, GameManager.instance.LuckyNumber);
        if(number == 0)
        {
            number = UnityEngine.Random.Range(0, GameManager.instance.LuckyNumber);
            if (number == 0)
            {
                GameObject item = Instantiate(GameManager.instance.godRing, transform.position, Quaternion.identity);
            }
            else
            {
                GameObject item = Instantiate(GameManager.instance.allItems[UnityEngine.Random.Range(0, GameManager.instance.allItems.Count)], transform.position, Quaternion.identity);
            }
        }
    }
    IEnumerator TakeDamageEffect()
    {
        spriteRenderer.color = Color.Lerp(Color.white, Color.red, 0.6f);
        yield return new WaitForSeconds(.5f);
        spriteRenderer.color = Color.white;
    }
    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeDamageEffect());
        currentHp -= damage;
    }
    public void UpdatingStats()
    {
        Hp = stats.maxHp;
        speed = stats.speed;
        attackSpeed = stats.attackSpeed;
        attackRange = stats.attackRange;
        ap = stats.ap;
        ad = stats.ad;
        energy = stats.energy;
        armor = stats.armor;

        sightRange = stats.sightRange;
        walkPointRangeX = stats.walkPointRangeX;
        walkPointRangeY = stats.walkPointRangeY;
    }
}

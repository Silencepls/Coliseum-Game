using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region variables

    // Lucky Number

    public int LuckyNumber;

    // UI Manager

    public Text WaveTxt;
    public int currentWave = 0;

    // Spikes

    [SerializeField] GameObject spikes;
    [SerializeField] Transform spikesHolder;

    // SpawnPoints

    public Transform[] spawnPoints;

    // Inventory

    public InteractableObject interactableObject;
    List<InteractableObject> interactableObjects = new List<InteractableObject>();
    List<InteractableObject> inventory = new List<InteractableObject>();

    public List<GameObject> allItems = new List<GameObject>();
    public GameObject godRing;

    // Enemy List

    public List<GameObject> enemyList = new List<GameObject>();
    public GameObject[] enemies;
    public GameObject miniBoss;

    #endregion
    private void Awake()
    {
        LuckyNumber = Mathf.Clamp(LuckyNumber, 1, 7);
        LuckyNumber = 7;
        if (instance == null)
        {
            instance = this;
        }
        SetSpikes();
    }
    void SetSpikes()
    {
        float x = 11;
        float y = 7;

        for (float i = -11.5f; i < x; i+= 2)
        {
            for (float j = -6.5f; j < y; j+= 2)
            {
                GameObject spikesGameObject = Instantiate(spikes, new Vector2(i, j), Quaternion.identity);
                spikesGameObject.transform.SetParent(spikesHolder);
                if (i < x - 1 && j < y - 3)
                {
                    GameObject spikesGameObject2 = Instantiate(spikes, new Vector2(i + 1, j + 1), Quaternion.identity);
                    spikesGameObject2.transform.SetParent(spikesHolder);
                }
            }
        }
    }
    private void Update()
    {
        if (enemyList.Count == 0)
        {
            UpdateWave();
        }

        // interactableObjects
        if (interactableObjects.Count == 0)
        {
            interactableObject = null;
        }
        else
        {
            interactableObject = interactableObjects[0];
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            IncreaseWave();
        }
    }

    // Text Management
    void IncreaseWave()
    {
        currentWave++;
        WaveTxt.text = "WAVE " + currentWave.ToString();
    }

    // Wave Management

    void StartWave()
    {
        if(currentWave == 11)
        {
            TimeManager.instance.WinScene();
        }
        for (int i = 0; i < currentWave + 4; i++)
        {
            GameObject _gameObjectEnemy = Instantiate(enemies[Random.Range(0, 2)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            enemyList.Add(_gameObjectEnemy);
        }
        if(currentWave % 5 == 0)
        {
            for (int i = 0; i < currentWave / 5; i++)
            {
                GameObject _miniBoss = Instantiate(miniBoss, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                enemyList.Add(_miniBoss);
            }
        }
    }
    void UpdateWave()
    {
        IncreaseWave();
        StartWave();
    }

    // Item Management

    public void AddToInteractableObjectsList(InteractableObject interactable)
    {
        interactableObjects.Add(interactable);
    }
    public void RemoveFromInteractableObjectsList(InteractableObject interactable)
    {
        interactableObjects.Remove(interactable);
    }
    public void AddToIventory(InteractableObject interactable)
    {
        inventory.Add(interactable);
    }

    // Effect Management

    public IEnumerator SetBurn(UpdateStats updateStats, int damage)
    {
        updateStats.burnDuration = 5;
        if (!updateStats.state.HasFlag(UpdateStats.State.Burning))
        {
            StartCoroutine(Burn(updateStats, damage));
            updateStats.state |= UpdateStats.State.Burning;
        }
        yield return null;
    }
    private IEnumerator Burn(UpdateStats updateStats, int damage)
    {
        GameObject burnEffect = Instantiate(EffectManager.instance.burnEffect, updateStats.gameObject.transform);
        while(updateStats.burnDuration > 0)
        {
            updateStats.burnDuration -= 1;
            updateStats.TakeDamage(damage);
            yield return new WaitForSeconds(1);
        }
        Destroy(burnEffect);
        updateStats.state &= ~UpdateStats.State.Burning;
    }

    public IEnumerator SetStun(UpdateStats updateStats, float duration)
    {
        if(duration > updateStats.stunDuration)
        {
            updateStats.stunDuration = duration;
        }
        if (!updateStats.state.HasFlag(UpdateStats.State.Stunned))
        {
            StartCoroutine(Stun(updateStats, duration));
            updateStats.state |= UpdateStats.State.Stunned;
        }
        yield return null;
    }
    private IEnumerator Stun(UpdateStats updateStats, float duration)
    {
        GameObject stunEffect = Instantiate(EffectManager.instance.stunEffect, updateStats.gameObject.transform);
        while(updateStats.stunDuration > 0)
        {
            updateStats.stunDuration -= 1;
            updateStats.speed = 0;
            yield return new WaitForSeconds(1);
        }
        updateStats.speed = updateStats.currentSpeed;
        updateStats.state &= ~UpdateStats.State.Stunned;
        Destroy(stunEffect);
    }

    public void SetShoot(UpdateStats updateStats, Transform startPosition, Transform destination, bool shouldDamageEnemies)
    {
        if (updateStats.canAttack && !updateStats.state.HasFlag(UpdateStats.State.Stunned))
        {
            StartCoroutine(GameManager.instance.Shoot(updateStats, startPosition, destination, shouldDamageEnemies));
        }
    }
    private IEnumerator Shoot(UpdateStats updateStats, Transform startPosition, Transform destination, bool shouldDamageEnemies)
    {
        updateStats.canAttack = false;

        while(updateStats.currentAttackSpeed > 0)
        {
            updateStats.currentAttackSpeed--;
            yield return new WaitForSeconds(1);
        }

        GameObject shoot = Instantiate(EffectManager.instance.shootEffect, startPosition.position, Quaternion.identity);
        Rigidbody2D rb = shoot.GetComponent<Rigidbody2D>();
        shoot.GetComponent<ShootingLogic>().SetTarget(updateStats, shouldDamageEnemies);
        Vector2 direction = ((Vector2)destination.position - (Vector2)startPosition.position).normalized * 450f;
        rb.AddForce(direction);

        updateStats.currentAttackSpeed = updateStats.attackSpeed;
        updateStats.canAttack = true;

        yield return null;
    }
    public void TransformShoot(UpdateStats updateStats, Transform startPosition, Transform[] startPositions, bool shouldDamageEnemies, bool activateCoroutine)
    {
        if (activateCoroutine)
        {
            if(updateStats.canAttack && !updateStats.state.HasFlag(UpdateStats.State.Stunned))
            {
                StartCoroutine(GameManager.instance.TransformShootCoroutine(updateStats, startPositions, shouldDamageEnemies));
            }
        }
        else
        {
            GameObject shoot = Instantiate(EffectManager.instance.shootEffect, startPosition.position, Quaternion.identity);
            Rigidbody2D rb = shoot.GetComponent<Rigidbody2D>();
            shoot.GetComponent<ShootingLogic>().SetTarget(updateStats, shouldDamageEnemies);
            rb.AddForce(startPosition.right * 450f);
        }
    }
    private IEnumerator TransformShootCoroutine(UpdateStats updateStats, Transform[] startPositions, bool shouldDamageEnemies)
    {
        updateStats.canAttack = false;

        while (updateStats.currentAttackSpeed > 0)
        {
            updateStats.currentAttackSpeed--;
            yield return new WaitForSeconds(1);
        }
        foreach (Transform startPosition in startPositions)
        {
            GameObject shoot = Instantiate(EffectManager.instance.shootEffect, startPosition.position, Quaternion.identity);
            Rigidbody2D rb = shoot.GetComponent<Rigidbody2D>();
            shoot.GetComponent<ShootingLogic>().SetTarget(updateStats, shouldDamageEnemies);
            rb.AddForce(startPosition.up * 450f);
        }

        updateStats.currentAttackSpeed = updateStats.attackSpeed;
        updateStats.canAttack = true;

        yield return null;
    }

    public IEnumerator KnockBack(UpdateStats updateStats, Vector2 target, Vector2 origin, float force, float duration)
    {
        Rigidbody2D rb = updateStats.gameObject.GetComponent<Rigidbody2D>();
        Vector2 direction = (target - origin).normalized;

        while (duration > 0)
        {
            rb.AddForce(direction * force);
            duration -= Time.deltaTime;
        }
        yield return null;
    }

    public IEnumerator Gravity(Rigidbody2D rb, Vector3 center, float force)
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, (Vector3)rb.position);
            rb.AddForce((center - (Vector3)rb.position) * (1/distance) * force);
            yield return null;
        }
    }

    public void LifeSteel(UpdateStats dealer, UpdateStats receiver)
    {
        float result = dealer.ad - receiver.armor;
        if (result < 0)
        {
            result = 0;
        }
        receiver.TakeDamage(result);
        dealer.TakeDamage(-result);
    }
    //public void TakeDamage(Stats dealer, Stats receiver)
    //{
    //    float result = dealer.ad - receiver.armor;
    //    if (result < 0)
    //    {
    //        result = 0;
    //    }
    //    receiver.TakeDamage(result);
    //}
}

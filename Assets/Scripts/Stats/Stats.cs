using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats")]
public class Stats : ScriptableObject
{
    [Header("HP")]
    public float maxHp;
    [Header("Strength")]
    public float energy;
    public float ad;
    public float ap;
    public float armor;
    [Header("Range")]
    public float attackRange;
    public float sightRange;
    [Header("Speed")]
    public float attackSpeed;
    public float speed;
    [Header("WalkPointXY")]
    public float walkPointRangeX;
    public float walkPointRangeY;
    [Header("Attack")]
    public bool alreadyAttacked;
}

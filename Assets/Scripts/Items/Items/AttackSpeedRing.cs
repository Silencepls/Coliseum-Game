using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedRing : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.attackSpeed += 1;
        Destroy(gameObject);
    }
}

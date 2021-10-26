using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodRing : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.fireRing = true;
        playerStats.gameObject.GetComponent<PlayerMovement>().goldenArmor = true;
        playerStats.Hp += 20;
        playerStats.ad += 1;
        playerStats.speed+= 1;
        playerStats.attackSpeed += 1;
        playerStats.TakeDamage(0.03f);
        Destroy(gameObject);
    }
}

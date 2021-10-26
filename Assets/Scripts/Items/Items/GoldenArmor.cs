using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenArmor : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.gameObject.GetComponent<PlayerMovement>().goldenArmor = true;
        playerStats.Hp += 20;
        Destroy(gameObject);
    }
}

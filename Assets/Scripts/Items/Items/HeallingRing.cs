using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeallingRing : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.heallingRing = true;
        Destroy(gameObject);
    }
}

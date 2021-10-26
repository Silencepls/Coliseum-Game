using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.TakeDamage(-25);
        Destroy(gameObject);
    }
}

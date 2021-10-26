using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.TakeDamage(-10);
        Destroy(gameObject);
    }
}

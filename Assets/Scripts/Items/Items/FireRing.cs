using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.fireRing = true;
        playerStats.ad += 1;
        Destroy(gameObject);
    }
}

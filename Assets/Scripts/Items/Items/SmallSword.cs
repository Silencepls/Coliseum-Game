using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSword : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.ad += 1;
        Destroy(gameObject);
    }
}

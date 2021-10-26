using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boots : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        playerStats.speed += 1;
        playerStats.currentSpeed = playerStats.speed;
        Destroy(gameObject);
    }
}

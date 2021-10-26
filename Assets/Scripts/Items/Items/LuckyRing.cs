using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyRing : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        GameManager.instance.LuckyNumber -= 1;
        Destroy(gameObject);
    }
}

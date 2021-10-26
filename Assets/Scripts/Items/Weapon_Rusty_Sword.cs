using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Rusty_Sword : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();

        GameManager.instance.AddToIventory(this);
        this.gameObject.SetActive(false);
        return;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    protected UpdateStats playerStats;
    Coroutine coroutine;
    private void Update()
    {
        if (GameManager.instance.interactableObject == this && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    protected virtual void Interact()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.AddToInteractableObjectsList(this);
            playerStats = collision.GetComponent<UpdateStats>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.RemoveFromInteractableObjectsList(this);
            playerStats = null;
        }
    }
}

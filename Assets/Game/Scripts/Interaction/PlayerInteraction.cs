using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private Collider2D playerInteractCollider;

    [SerializeField] private List<IInteractable> interactableObject = new List<IInteractable>();
    private bool canInteract = true;
    private bool inInteractRange = false;
    private int count = 0;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        if (canInteract && inInteractRange)
        {
            TextUpdater();

            if (Input.GetKeyDown(KeyCode.F))
            {
                Interact();
            }
        }
    }

    private void DisableInteract(bool value) { canInteract = value; } //to disable interaction subscribe this function to any relevant events in OnEnable and OnDisable.

    private void TextUpdater() //updates interaction text if the interactable object can be used and is NOT null.
    {
        if (interactableObject[0].Interactable && interactableObject != null)
        {
           // interactText.text = $"{interactableObject[0].InteractText} (F)";
            //interactText.enabled = true;
        }
        else //or else the text will be disabled.
        {
          //  interactText.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            interactableObject.Add(collision.GetComponent<IInteractable>());
            inInteractRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            if (interactableObject.Contains(collision.GetComponent<IInteractable>()))
            {
                interactableObject.Remove(collision.GetComponent<IInteractable>());
            }
            if (count == 0) { inInteractRange = false; }
        }
    }

    private void Interact()
    {
        if (canInteract && interactableObject.Count > 0)
        {
            if (interactableObject[0].Interactable)
            {
                interactableObject[0].Interact();
            }
            else
            {
                return;
            }
        }
    }
}
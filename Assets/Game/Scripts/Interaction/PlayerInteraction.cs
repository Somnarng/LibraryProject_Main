using EPOOutline;
using System.Collections.Generic;
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
    public IInteractable object1;

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
            if (Input.GetButtonDown("Interact"))
            {
                Interact();
            }
        }
    }

    private void DisableInteract(bool value) { canInteract = value; } //to disable interaction subscribe this function to any relevant events in OnEnable and OnDisable.

    private void TextUpdater() //updates interaction text if the interactable object can be used and is NOT null.
    {
        if (object1 == null)
        {
            return;
        }
        else if (object1.Interactable)
        {
            //interactText.text = $"{object1.InteractText} (F)";
            //interactText.enabled = true;
            if (object1.objectTransform.GetComponentInParent<Outlinable>() != null) { object1.objectTransform.GetComponentInParent<Outlinable>().enabled = true; }
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
            count++;
            interactableObject.Add(collision.GetComponent<IInteractable>());
            inInteractRange = true;
            FindTarget();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            count--;
            if (interactableObject.Contains(collision.GetComponent<IInteractable>()))
            {
                interactableObject.Remove(collision.GetComponent<IInteractable>());
                FindTarget();
            }
            if (count == 0)
            {
                inInteractRange = false;
                if (collision.transform.GetComponentInParent<Outlinable>() != null) { collision.transform.GetComponentInParent<Outlinable>().enabled = false; }
                object1 = null;
            }
        }
    }

    private void Interact()
    {
        if (canInteract && object1 != null)
        {
            if (object1.Interactable)
            {
                object1.Interact();
            }
            else
            {
                return;
            }
        }
    }

    void FindTarget() //looks for the gameobject in interactableObject with the smallest distance from the player. this gameobject will be the "selected" object for interactions.
    {
        float lowestDist = Mathf.Infinity;

        for (int i = 0; i < interactableObject.Count; i++)
        {
            float dist = Vector3.Distance(interactableObject[i].objectTransform.position, transform.position);

            if (dist < lowestDist && interactableObject[i].Interactable)
            {
                lowestDist = dist;
                if (object1 != interactableObject[i])
                {
                    if (object1 != null && object1.objectTransform.GetComponentInParent<Outlinable>() != null) { object1.objectTransform.GetComponentInParent<Outlinable>().enabled = false; }
                    object1 = interactableObject[i];
                    TextUpdater();
                }
                Debug.Log(object1);
            }
        }
    }
}
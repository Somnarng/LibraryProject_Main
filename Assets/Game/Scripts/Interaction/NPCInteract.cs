using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NPCInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable;
    [SerializeField] private string interactText;

    [Header("Relationships")]
    [SerializeField] private string conversationToTrigger;

    public void Interact()
    {
        DialogueManager.StartConversation(conversationToTrigger);
    }
    public bool Interactable { get { return interactable; } set { interactable = value; } }
    public string InteractText { get { return interactText; } set { interactText = value; } }
}

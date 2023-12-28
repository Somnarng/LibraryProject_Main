using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableTest : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable;
    [SerializeField] private string interactText;

    public void Interact()
    {
        gameObject.SetActive(false);
    }
    public bool Interactable { get { return interactable; } set { interactable = value; } }
    public string InteractText { get { return interactText; } set { interactText = value; } }
    public Transform objectTransform { get => gameObject.transform; }
}

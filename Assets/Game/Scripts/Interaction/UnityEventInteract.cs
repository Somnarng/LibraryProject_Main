using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UnityEventInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable;
    [SerializeField] private string interactText;
    [SerializeField] private UnityEvent unityEvent;
    [SerializeField] private float delay = 0.1f;
    public void Interact()
    {
        Invoke("EventTrigger", delay);
    }

   public void EventTrigger()
    {
        unityEvent?.Invoke();
    }

    public bool Interactable { get { return interactable; } set { interactable = value; } }
    public string InteractText { get { return interactText; } set { interactText = value; } }
    public Transform objectTransform { get => gameObject.transform; }
}
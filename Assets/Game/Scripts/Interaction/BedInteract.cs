using MoreMountains.Tools;
using System.Collections;
using UnityEngine;

public class BedInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable;
    [SerializeField] private string interactText;
    [SerializeField] private TimeManager.TimeSlot timeToSet;
    public void Interact()
    {
        StartCoroutine(Sleep());
    }

    public IEnumerator Sleep()
    {
        Debug.Log("Sleep Triggered");
        MMFadeInEvent.Trigger(0.5f, MMTweenType.DefaultEaseInCubic);
        TimeManager.Instance.Pause();
        TimeManager.Instance.ProgressDay();
        TimeManager.Instance.SetTime(timeToSet);
        yield return new WaitForSeconds(0.7f);
        MMFadeOutEvent.Trigger(0.5f, MMTweenType.DefaultEaseInCubic);
        TimeManager.Instance.Resume();
        yield return null;
    }
    public bool Interactable { get { return interactable; } set { interactable = value; } }
    public string InteractText { get { return interactText; } set { interactText = value; } }
    public Transform objectTransform { get => gameObject.transform; }
}
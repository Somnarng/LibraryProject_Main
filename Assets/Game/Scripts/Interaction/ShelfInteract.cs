using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable;
    [SerializeField] private string interactText;
    

    public void Interact()
    {
        StartCoroutine(OpenShelf());
    }

    public IEnumerator OpenShelf()
    {
        yield return null;
    }

    /**
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
    }**/

    public bool Interactable { get { return interactable; } set { interactable = value; } }
    public string InteractText { get { return interactText; } set { interactText = value; } }
    public Transform objectTransform { get => gameObject.transform; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

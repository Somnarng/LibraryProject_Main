using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public bool Interactable { get; set; } //whether or not this trigger can be interacted with at the current moment
    public string InteractText { get; set; } //text to display when interacting with this trigger
}
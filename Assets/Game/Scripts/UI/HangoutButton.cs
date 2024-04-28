using PixelCrushers.DialogueSystem;
using UnityEngine;

public class HangoutButton : MonoBehaviour
{
    string currentActor;
    bool hangoutStart = false;
    public void HangoutStart()
    {
        currentActor = DialogueManager.instance.currentConversationState.subtitle.speakerInfo.nameInDatabase;
        Debug.Log(currentActor);
        if (DialogueManager.instance.ConversationHasValidEntry(currentActor + "/Hangout"))
        {
            DialogueManager.instance.StopConversation();
            DialogueManager.instance.StartConversation(currentActor + "/Hangout");
            hangoutStart = true;
        }
        else { Debug.Log("No conversation with " + currentActor + "/Hangout name was found!"); }
    }
    private void OnDisable()
    {
        if (hangoutStart)
        {
            FindAnyObjectByType<BedInteract>().StartCoroutine("DebugProgressTime");
            hangoutStart = false;
        }
    }
}
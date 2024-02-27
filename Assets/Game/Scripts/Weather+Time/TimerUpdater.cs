using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerUpdater : MonoBehaviour
{
    public TMP_Text Timer_Text;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateTimeManager;   
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= UpdateTimeManager;
    }
    private void UpdateTimeManager(Scene scene, LoadSceneMode mode)
    {
        TimeManager.Instance.TimerText = this;
        TimeManager.Instance.UpdateTimerText();
    }
    public void UpdateText(string text)
    {
        Timer_Text.text = text;
    }
}
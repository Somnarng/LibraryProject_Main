using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUpdater : MonoBehaviour
{
    public TMP_Text Timer_Text;
    void Update()
    {
        Timer_Text.text = TimeManager.Instance.CurrentTimeAsString();
    }
}
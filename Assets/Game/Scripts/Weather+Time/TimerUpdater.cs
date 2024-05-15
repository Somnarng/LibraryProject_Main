using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerUpdater : MonoBehaviour
{
    public TMP_Text Timer_Text;
    public TMP_Text Day_Text;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateTimeManager;
        TimeManager.TimePassed += UpdateText;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= UpdateTimeManager;
        TimeManager.TimePassed -= UpdateText;
    }
    private void UpdateTimeManager(Scene scene, LoadSceneMode mode)
    {
        UpdateText();
    }
    private void UpdateText()
    {
        Timer_Text.text = TimeManager.Instance.currentTimeSlot.ToString();
        Day_Text.text = TimeManager.Instance.currentDate;
    }
}
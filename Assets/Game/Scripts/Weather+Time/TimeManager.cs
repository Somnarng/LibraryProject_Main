using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TimeManager is the entry point to all the game system. It's execution order is set very low to make sure
/// its Awake function is called as early as possible so the instance if valid on other Scripts. 
/// </summary>
[DefaultExecutionOrder(-9999)]
public class TimeManager : Singleton<TimeManager>, IDataPersistence
{
    public DayCycleHandler DayCycleHandler { get; set; }
    public WeatherSystem WeatherSystem { get; set; }
    public TimerUpdater TimerText { get; set; }

    // Will return the ratio of time for the current day between 0 (00:00) and 1 (23:59).
    public float CurrentDayRatio => m_CurrentTimeOfTheDay / DayDurationInSeconds;

    [Header("Time settings")]
    [Min(1.0f)]
    public float DayDurationInSeconds;
    public float StartingTime = 0.0f;
    public bool MilitaryTime = true;

    [Header("Day settings")]
    public int DaysInMonth;
    public int MonthsInYear;

    private bool m_IsTicking;

    private List<DayEventHandler> m_EventHandlers = new();

    private float m_CurrentTimeOfTheDay;
    private int m_CurrentDayOfMonth = 1;
    private int m_CurrentMonthOfYear = 1;
    private int m_CurrentYear = 1;

    private int ratioMultiplier = 1;

    protected override void OnAwake()
    {
        m_IsTicking = true;

        m_CurrentTimeOfTheDay = StartingTime;

        //we need to ensure that we don't have a day length at 0, otherwise we will get stuck into infinite loop in update
        //(and a day with 0 length makes no sense)
        if (DayDurationInSeconds <= 0.0f)
        {
            DayDurationInSeconds = 1.0f;
            Debug.LogError("The day length on the GameManager is set to 0, the length need to be set to a positive value");
        }
    }

    private void Start()
    {
        m_CurrentTimeOfTheDay = StartingTime;
    }

    private void Update()
    {
        if (m_IsTicking)
        {
            float previousRatio = CurrentDayRatio;
            m_CurrentTimeOfTheDay += Time.deltaTime;

            while (m_CurrentTimeOfTheDay > DayDurationInSeconds)
                m_CurrentTimeOfTheDay -= DayDurationInSeconds;

            foreach (var handler in m_EventHandlers)
            {
                foreach (var evt in handler.Events)
                {
                    bool prev = evt.IsInRange(previousRatio);
                    bool current = evt.IsInRange(CurrentDayRatio);

                    if (prev && !current)
                    {
                        evt.OffEvent.Invoke();
                    }
                    else if (!prev && current)
                    {
                        evt.OnEvents.Invoke();
                    }
                }
            }

            if (DayCycleHandler != null)
                DayCycleHandler.Tick(CurrentDayRatio);
            if (TimerText != null)
                TimerText.UpdateText(CurrentTimeAsString());
        }
    }

    public void Pause()
    {
        m_IsTicking = false;
    }

    public void Resume()
    {
        m_IsTicking = true;
    }

    public void SaveData(ref PlayerStats data)
    {
        data.timeOfTheDay = m_CurrentTimeOfTheDay;
        data.dayOfMonth = m_CurrentDayOfMonth;
        data.monthOfYear = m_CurrentMonthOfYear;
        data.year = m_CurrentYear;
    }

    public void LoadData(PlayerStats data)
    {
        m_CurrentTimeOfTheDay = data.timeOfTheDay;
        StartingTime = m_CurrentTimeOfTheDay;
        m_CurrentDayOfMonth = data.dayOfMonth;
        m_CurrentMonthOfYear = data.monthOfYear;
        m_CurrentYear = data.year;
    }

    public void ProgressDay()
    {
        m_CurrentDayOfMonth++;
        if (m_CurrentDayOfMonth > DaysInMonth)
        {
            m_CurrentDayOfMonth = 1;
            m_CurrentMonthOfYear++;
        }
        if (m_CurrentMonthOfYear > MonthsInYear)
        {
            m_CurrentMonthOfYear = 1;
            m_CurrentYear++;
        }
        Debug.Log("Day:" + m_CurrentDayOfMonth + " Month:" + m_CurrentMonthOfYear + " Year:" + m_CurrentYear);
        UpdateDialogueVariables();
    }

    public void UpdateDialogueVariables()
    {
        DialogueLua.SetVariable("time.Day", m_CurrentDayOfMonth);
        DialogueLua.SetVariable("time.Year", m_CurrentYear);

        switch (m_CurrentMonthOfYear)
        {
            case 1:
                DialogueLua.SetVariable("time.Month", "Spring");
                break;
            case 2:
                DialogueLua.SetVariable("time.Month", "Summer");
                break;
            case 3:
                DialogueLua.SetVariable("time.Month", "Fall");
                break;
            case 4:
                DialogueLua.SetVariable("time.Month", "Winter");
                break;
            default:
                DialogueLua.SetVariable("time.Month", "ERROR:MONTH_UNKNOWN");
                break;
        }
    }

    public void SetTime(float time)
    {
        m_CurrentTimeOfTheDay = time;
    }

    /// <summary>
    /// Will return the current time as a string in format of "xx:xx" or "xx:xx AM/PM" if not using military time.
    /// </summary>
    /// <returns></returns>
    public string CurrentTimeAsString()
    {
        if (!MilitaryTime) { return GetTimeAsString(CurrentDayRatio); }
        else
        {
            return GetMilitaryTimeAsString(CurrentDayRatio);
        }
    }

    /// <summary>
    /// Return in the format "xx:xx" the given ration (between 0 and 1) of time
    /// </summary>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public static string GetMilitaryTimeAsString(float ratio)
    {
        var hour = GetHourFromRatio(ratio);
        var minute = GetMinuteFromRatio(ratio);
        return $"{hour}:{minute:00}";
    }

    public static string GetTimeAsString(float ratio)
    {
        var hour = GetHourFromRatio(ratio);
        var minute = GetMinuteFromRatio(ratio);
        string period;
        if (hour >= 12) { period = "PM"; TimeManager.Instance.ratioMultiplier = 2; }
        else { period = "AM"; TimeManager.Instance.ratioMultiplier = 1; }
        if (hour > 12)
        {
            hour -= 12;
        }
        else if (hour == 0) { hour = 12; }
        return $"{hour}:{minute:00} " + period;
    }

    public static string ConvertCustomTimeToString(float ratio)
    {
        var hour = GetHourFromRatio(ratio);
        var minute = GetMinuteFromRatio(ratio);
        return $"{hour}:{minute:00}";
    }

    public static int GetHourFromRatio(float ratio)
    {
        var time = ratio * 24.0f;
        var hour = Mathf.FloorToInt(time);

        return hour;
    }

    public static int GetMinuteFromRatio(float ratio)
    {
        var time = ratio * 24.0f;
        var minute = Mathf.FloorToInt((time - Mathf.FloorToInt(time)) * 60.0f);

        return minute;
    }

    public static void RegisterEventHandler(DayEventHandler handler)
    {
        foreach (var evt in handler.Events)
        {
            if (evt.IsInRange(TimeManager.Instance.MilitaryTime ? TimeManager.Instance.CurrentDayRatio : TimeManager.Instance.CurrentDayRatio * TimeManager.Instance.ratioMultiplier))//multiplies the current day ration by 2 if not using military time.
            {
                evt.OnEvents.Invoke();
            }
            else
            {
                evt.OffEvent.Invoke();
            }
        }

        Instance.m_EventHandlers.Add(handler);
    }

    public static void RemoveEventHandler(DayEventHandler handler)
    {
        Instance?.m_EventHandlers.Remove(handler);
    }
}

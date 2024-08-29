using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using AuroraFPSRuntime.CoreModules.Pattern;

[DefaultExecutionOrder(-9999)]
public class TimeManager : Singleton<TimeManager>, IDataPersistence
{
    public DayCycleHandler DayCycleHandler { get; set; }
    public WeatherSystem WeatherSystem { get; set; }

    public static Action TimePassed;

    // Will return the ratio of time for the current day between 0 (00:00) and 1 (23:59).
    public float CurrentDayRatio;

    [Header("Time settings")]
    [Min(1.0f)]
    public float DayDurationInSeconds;
    public float StartingTime = 0.36f;
    public TimeSlot StartingTimeSlot;
    //public bool MilitaryTime = true;

    [Header("Day settings")]
    public int DaysInMonth;
    public int MonthsInYear;

    private List<DayEventHandler> m_EventHandlers = new();

    //private float m_CurrentTimeOfTheDay;
    private int m_CurrentDayOfMonth = 1;
    private int m_CurrentMonthOfYear = 1;
    private int m_CurrentYear = 1;
    public TimeSlot currentTimeSlot;
    public string currentDate;

    //private int ratioMultiplier = 1;

    protected override void Awake()
    {

        currentTimeSlot = StartingTimeSlot;

        if (DayDurationInSeconds <= 0.0f)
        {
            DayDurationInSeconds = 1.0f;
            Debug.LogError("The day length on the GameManager is set to 0, the length need to be set to a positive value");
        }
    }

    private void Start()
    {
        //m_CurrentTimeOfTheDay = StartingTime;
        currentTimeSlot = StartingTimeSlot;
        currentDate = "Day:" + m_CurrentDayOfMonth + " Month:" + m_CurrentMonthOfYear + " Year:" + m_CurrentYear;
        TimePassed?.Invoke();
    }

    public void SaveData(ref PlayerStats data)
    {
        data.dayOfMonth = m_CurrentDayOfMonth;
        data.monthOfYear = m_CurrentMonthOfYear;
        data.year = m_CurrentYear;
    }

    public void LoadData(PlayerStats data)
    {
        m_CurrentDayOfMonth = data.dayOfMonth;
        m_CurrentMonthOfYear = data.monthOfYear;
        m_CurrentYear = data.year;
        UpdateDialogueVariables();
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
        currentDate = "Day:" + m_CurrentDayOfMonth + " Month:" + m_CurrentMonthOfYear + " Year:" + m_CurrentYear;
        UpdateDialogueVariables();
        TimePassed?.Invoke();
        DataPersistenceManager.GetRuntimeInstance().SaveGame();
    }
    public void ProgressTime() //call this function to progress to the next timeslot
    {
        switch (currentTimeSlot)
        {
            case TimeSlot.Morning:
                currentTimeSlot = TimeSlot.Noon;
                CurrentDayRatio = 0.5f;
                break;
            case TimeSlot.Noon:
                currentTimeSlot = TimeSlot.Afternoon;
                CurrentDayRatio = .63f;
                break;
            case TimeSlot.Afternoon:
                currentTimeSlot = TimeSlot.Evening;
                CurrentDayRatio = .76f;
                break;
            case TimeSlot.Evening:
                currentTimeSlot = TimeSlot.Morning;
                CurrentDayRatio = 0.36f;
                ProgressDay();
                Debug.Log("Day Passed!");
                break;
            default:
                currentTimeSlot = TimeSlot.Morning;
                CurrentDayRatio = 0.36f;
                Debug.Log("ERROR, ENUM FOR TIMESLOT PROGRESSION BROKEN.");
                break;
        }
        Debug.Log("Time is " + currentTimeSlot);
        TimePassed?.Invoke(); //invoke timepassed event
        DayCycleHandler.UpdateLight(CurrentDayRatio);
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

    public void SetTime(TimeSlot time)
    {
        currentTimeSlot = time;
    }
    public string CurrentTimeAsString()
    {
        return (currentTimeSlot.ToString());
    }
    public enum TimeSlot
    {
        Morning,
        Noon,
        Afternoon,
        Evening
    }
}

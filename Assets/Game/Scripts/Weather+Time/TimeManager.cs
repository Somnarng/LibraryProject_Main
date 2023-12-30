using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// The GameManager is the entry point to all the game system. It's execution order is set very low to make sure
/// its Awake function is called as early as possible so the instance if valid on other Scripts. 
/// </summary>
[DefaultExecutionOrder(-9999)]
public class TimeManager : MonoBehaviour
{
    private static TimeManager s_Instance;


#if UNITY_EDITOR
    //As our manager run first, it will also be destroyed first when the app will be exiting, which lead to s_Instance
    //to become null and so will trigger another instantiate in edit mode (as we dynamically instantiate the Manager)
    //so this is set to true when destroyed, so we do not reinstantiate a new one
    private static bool s_IsQuitting = false;
#endif
    public static TimeManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || s_IsQuitting)
                return null;

            if (s_Instance == null)
            {
                //in editor, we can start any scene to test, so we are not sure the game manager will have been
                //created by the first scene starting the game. So we load it manually. This check is useless in
                //player build as the 1st scene will have created the GameManager so it will always exists.
                Instantiate(Resources.Load<TimeManager>("TimeManager"));
            }
#endif
            return s_Instance;
        }
    }
    public DayCycleHandler DayCycleHandler { get; set; }
    public WeatherSystem WeatherSystem { get; set; }

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

    private void Awake()
    {
        s_Instance = this;
        DontDestroyOnLoad(gameObject);

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

#if UNITY_EDITOR
    private void OnDestroy()
    {
        s_IsQuitting = true;
    }
#endif

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
                DayCycleHandler.Tick();
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

    public void Save(ref TimeManagerSaveData data)
    {
        data.TimeOfTheDay = m_CurrentTimeOfTheDay;
        data.DayOfMonth = m_CurrentDayOfMonth;
        data.MonthOfYear = m_CurrentMonthOfYear;
        data.Year = m_CurrentYear;
    }

    public void Load(TimeManagerSaveData data)
    {
        m_CurrentTimeOfTheDay = data.TimeOfTheDay;
        StartingTime = m_CurrentTimeOfTheDay;
        m_CurrentDayOfMonth = data.DayOfMonth;
        m_CurrentMonthOfYear = data.MonthOfYear;
        m_CurrentYear = data.Year;
    }

    public void ProgressDay()
    {
        m_CurrentDayOfMonth++;
        if(m_CurrentDayOfMonth > DaysInMonth)
        {
            m_CurrentDayOfMonth = 1;
            m_CurrentMonthOfYear++;
        }
        if(m_CurrentMonthOfYear > MonthsInYear)
        {
            m_CurrentMonthOfYear = 1;
            m_CurrentYear++;
        }
        Debug.Log("Day:"+ m_CurrentDayOfMonth+ " Month:" + m_CurrentMonthOfYear + " Year:" +m_CurrentYear);
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
        return GetTimeAsString(CurrentDayRatio);
    }

    /// <summary>
    /// Return in the format "xx:xx" the given ration (between 0 and 1) of time
    /// </summary>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public static string GetTimeAsString(float ratio)
    {
        var hour = GetHourFromRatio(ratio);
        var minute = GetMinuteFromRatio(ratio);
        if (TimeManager.Instance.MilitaryTime == false) //if NOT using a 24hour clock, remove 12 hours if time goes over 12 to match standard clocks. Also adds AM/PM to time.
        {
            string period;
            if (hour >= 12) { period = "PM"; TimeManager.Instance.ratioMultiplier = 2; }
            else { period = "AM"; TimeManager.Instance.ratioMultiplier = 1; }
            if (hour > 12)
            {
                hour -= 12;
            }
            else if(hour == 0) { hour = 12; }
            return $"{hour}:{minute:00} " + period;

        }
        return $"{hour}:{minute:00}"; 
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

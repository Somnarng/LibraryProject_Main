using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class ScheduleItem
{
    public int Year;
    public int Month;
    public int Day;
    public int Weekday;
    public int Hour;
    public string Scene;
    public Vector3 Position;
    public string Activity;
    public string FromExit;
}
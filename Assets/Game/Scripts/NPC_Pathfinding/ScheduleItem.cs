using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class ScheduleItem
{
    public int Day;
    public int Month;
    public int Weekday;
    public TimeManager.TimeSlot Slot;
    public string Scene;
    public Routine[] Routine;
}


[System.Serializable]
public class Routine
{
    public Vector3 Position;
    public string Activity;
    public bool completed;
    public float timerToChange; //will move to this position after the timer hits this number.
}
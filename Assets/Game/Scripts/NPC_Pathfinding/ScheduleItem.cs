using UnityEngine;

public class ScheduleItem
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public int Weekday { get; set; }
    public int Hour { get; set; }
    public string Scene { get; set; }
    public Vector3 Position { get; set; }
    public string PositionType { get; set; }
    public string Activity { get; set; }
    public string FromExit { get; set; }
}

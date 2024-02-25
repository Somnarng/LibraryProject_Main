using Pathfinding;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(50)]
public class NPC_Manager : MonoBehaviour
{
    public string Name;
    public NPCStateModel State { get; set; }
    public NPCSceneManager GameManager { get; set; }
    public bool TeleportToScheduleItem { get; set; }

    ScheduleItem currentScheduleItem;

    Seeker seeker;
    Path currentPath;
    int currentWaypoint = 0;

    [SerializeField]
    TimeManager.TimeSlot currentHour;

    void OnEnable()
    {
        GameManager = NPCSceneManager.Instance;
        seeker = GetComponent<Seeker>();
        State = NPCSceneManager.Instance.Game.NPCS.Find(x => x.Name == Name);
        CheckSchedule();
        TimeManager.TimePassed += CheckSchedule;
        if (State == null) { enabled = false; }
    }
    private void OnDisable()
    {
        TimeManager.TimePassed -= CheckSchedule;
    }

    void Update()
    {
      //  CheckRoutine();
      //  HandleMove();
    }

    void HandleMove()
    {
        if (currentPath != null)
        {
            if (currentWaypoint >= currentPath.vectorPath.Count)
            {
                currentPath = null;
                currentWaypoint = 0;
                OnScheduledPathComplete();
                return;
            }
        }
    }

    void CheckSchedule()
    {
        if (GameManager.Time.currentTimeSlot != currentHour)
        {
            State.routinePosition = 0; //reset routine counter
            currentHour = GameManager.Time.currentTimeSlot;

            var d = GameManager.Game.dayOfMonth;
            var wd = GameManager.Game.WeekDay;

            var item = State.LifetimeSchedule.FirstOrDefault(s => s.Day == d && s.Slot == currentHour);
            if (item == null)
            {
                item = State.WeeklySchedule.FirstOrDefault(s => s.Weekday == wd && s.Slot == currentHour);
            }
            if (item != null)
            {
                currentScheduleItem = item;
                if (currentScheduleItem.Scene == GameManager.Game.Scene.Name) //if the current scene is the same as the scene where the first routine for this slot is located, go THERE
                {
                    transform.position = item.Routine[0].Position;
                    OnScheduledPathComplete();
                }
                else
                {
                    gameObject.SetActive(false); //or else, destroy
                }
                foreach(Routine routine in currentScheduleItem.Routine)
                {
                    routine.completed = false; //reset completion bool
                }
            }
        }
    }

    void CheckRoutine()
    {
        var sceneModel = NPCSceneManager.Instance.Game.Scenes.Where(s => s.Name == SceneManager.GetActiveScene().name).FirstOrDefault(); //finds scene with name that matches the active scene, ticks that timer.
        if (State.Scene == sceneModel.Name)
        {
            //check timer of scenemodel, if timer goes over routine timer, start next routine path. if last routine path, stop checking.
            foreach (Routine routine in currentScheduleItem.Routine)
            {
                if (TimeManager.Instance.sceneModel.NPCRoutineTimer > routine.timerToChange && routine.completed == false && currentPath == null)
                {
                    if(State.routinePosition < currentScheduleItem.Routine.Count()) State.routinePosition++;//increase position in routine list and set completion to true
                    routine.completed = true;
                    seeker.StartPath(this.transform.position, currentScheduleItem.Routine[State.routinePosition].Position, OnScheduledPathReady);
                }
            }
        }
    }

    void OnScheduledPathReady(Path p)
    {
        currentPath = p;
        currentWaypoint = 0;
    }

    void OnScheduledPathComplete()
    {
        State.Scene = currentScheduleItem.Scene;
        State.Position = currentScheduleItem.Routine[State.routinePosition].Position;
        State.Activity = currentScheduleItem.Routine[State.routinePosition].Activity;
        Debug.Log(State.routinePosition);
        SaveStateToGameManager();
        seeker.CancelCurrentPathRequest();
        currentWaypoint = 0;
    }

    void SaveStateToGameManager()
    {
        var me = GameManager.Game.NPCS.FirstOrDefault(npc => npc.Name == this.Name);
        me = State;
    }
}
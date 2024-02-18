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

        if (State == null) { enabled = false; }
    }

    void Update()
    {
        // Dummy start path toward player
        if (Input.GetKeyDown("p"))
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            currentScheduleItem = new ScheduleItem
            {
                Scene = GameManager.Game.Scene.Name
            };
            seeker.StartPath(this.transform.position, player.transform.position, OnScheduledPathReady);
        }

        CheckSchedule();
        HandleMove();

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
            currentHour = GameManager.Time.currentTimeSlot;

            var h = GameManager.Time.currentTimeSlot;
            var d = GameManager.Game.dayOfMonth;
            var wd = GameManager.Game.WeekDay;

            var item = State.LifetimeSchedule.FirstOrDefault(s => s.Day == d && s.Slot == h);
            if (item == null)
            {
                item = State.WeeklySchedule.FirstOrDefault(s => s.Weekday == wd && s.Slot == h);
            }
            if (item != null)
            {
                currentScheduleItem = item;
                if (currentScheduleItem.Scene == GameManager.Game.Scene.Name) //if the current scene is the same as the scene where the first routine for this slot is located, go THERE
                {
                    transform.position = item.Routine[0].Position;
                    TeleportToScheduleItem = false;
                    OnScheduledPathComplete();
                }
                else
                {
                    gameObject.SetActive(false); //or else, destroy
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

            seeker.StartPath(this.transform.position, currentScheduleItem.Routine[State.routinePosition + 1].Position, OnScheduledPathReady);
        }
    }


    void OnScheduledPathReady(Path p)
    {
        currentPath = p;
        currentWaypoint = 0;
    }

    void OnScheduledPathComplete()
    {
        State.routinePosition++;
        State.Scene = currentScheduleItem.Scene;
        State.Position = currentScheduleItem.Routine[State.routinePosition].Position;
        State.Activity = currentScheduleItem.Routine[State.routinePosition].Activity;
        SaveStateToGameManager();

        if (currentScheduleItem.Scene != GameManager.Game.Scene.Name)
        {
            Destroy(this.gameObject);
        }
        else
        {
            currentWaypoint = 0;
        }
    }

    void SaveStateToGameManager()
    {
        var me = GameManager.Game.NPCS.FirstOrDefault(npc => npc.Name == this.Name);
        me = State;
    }
}
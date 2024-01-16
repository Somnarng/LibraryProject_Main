using MoreMountains.TopDownEngine;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
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
    int currentHour = -1;

    void Start()
    {

        State = GameManager.Game.NPCs.FirstOrDefault(n => n.Name == Name);
        seeker = GetComponent<Seeker>();
        var npc = GameManager.Game.NPCs.FirstOrDefault(n => n.Name == Name);
        State.LifetimeSchedule = npc.LifetimeSchedule;
        State.WeeklySchedule = npc.WeeklySchedule;

        if (State == null) { enabled = false; }

    }

    void Update()
    {

        // Dummy start path toward player
        if (Input.GetKeyDown("p"))
        {
            var player = GameObject.Find("Player");
            currentScheduleItem = new ScheduleItem
            {
                Activity = "Wait",
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
        if (GameManager.Time.hourOfDay != currentHour)
        {
            currentHour = GameManager.Time.hourOfDay;

            var h = GameManager.Time.hourOfDay;
            var d = GameManager.Game.dayOfMonth;
            var wd = GameManager.Game.WeekDay;

            var item = State.LifetimeSchedule.FirstOrDefault(s => s.Day == d && s.Hour == h);
            if (item == null)
            {
                item = State.WeeklySchedule.FirstOrDefault(s => s.Weekday == wd && s.Hour == h);
            }
            if (item != null)
            {
                currentScheduleItem = item;
                if (currentScheduleItem.Scene == GameManager.Game.Scene.Name)
                {
                    if (TeleportToScheduleItem)
                    {
                        transform.position = item.Position;
                        TeleportToScheduleItem = false;
                        OnScheduledPathComplete();
                    }
                    else
                    {
                        seeker.StartPath(transform.position, currentScheduleItem.Position, OnScheduledPathReady);
                    }

                }
                else
                {
                    var exit = GameManager.Game.Scene.Exits.FirstOrDefault(e => e.To == currentScheduleItem.Scene);
                    if (exit != null)
                    {
                        seeker.StartPath(transform.position, exit.Position, OnScheduledPathReady);
                    }
                    else
                    {
                        seeker.StartPath(transform.position, GameManager.Game.Scene.Exits.FirstOrDefault().Position, OnScheduledPathReady);
                    }
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
        State.Position = currentScheduleItem.Position;
        State.Activity = currentScheduleItem.Activity;
        SaveStateToGameManager();

        if (currentScheduleItem.Scene != GameManager.Game.Scene.Name)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
        else
        {
            currentWaypoint = 0;
        }
    }

    void SaveStateToGameManager()
    {
        var me = GameManager.Game.NPCs.FirstOrDefault(npc => npc.Name == this.Name);
        me = State;
    }
}
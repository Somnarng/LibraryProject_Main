using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCSceneManager : Singleton<NPCSceneManager>, IDataPersistence
{

    public PlayerStats Game;
    public TimeManager Time;
    public DefaultGame DGame;

    int currentHour = -1;

    // Use this for initialization

    void Start()
    {
        Time = TimeManager.Instance;
        DGame = new DefaultGame(Game);
    }

    private void Update()
    {
        HandleTime();
    }

    public void LoadData(PlayerStats data)
    {
        Game.NPCS = data.NPCS;
        Game.Scenes = data.Scenes;
        Game.GlobalFlags = data.GlobalFlags;
        Game.Scene = data.Scene;
        //Set all scheduled task items for those currently moving to their place
        foreach (var npc in Game.NPCS)
        {
            if (npc.Scene == Game.Scene.Name)
            {
                var item = npc.LifetimeSchedule.FirstOrDefault(s => s.Day == Game.dayOfMonth && s.Hour == Time.hourOfDay && s.Month == Game.monthOfYear);
                if (item == null)
                {
                    item = npc.WeeklySchedule.FirstOrDefault(s => s.Weekday == Game.WeekDay && s.Hour == Time.hourOfDay);
                }
                if (item != null)
                {
                    npc.Activity = item.Activity;
                    npc.Position = item.Position;
                    npc.Scene = item.Scene;
                }
            }
        }

        Game.Scene = Game.Scenes.Where(s => s.Name == SceneManager.GetActiveScene().buildIndex.ToString()).FirstOrDefault();
        DataLoaded();
    }

    void DataLoaded()
    {
        NPC_Manager[] allNPCs = UnityEngine.Object.FindObjectsOfType<NPC_Manager>(true);
        foreach (var c in allNPCs)
        {
            if (c.isActiveAndEnabled)
            {
                // Enable hard-coded npcs
                if (c.TryGetComponent<NPC_Manager>(out var npc))
                {
                    npc.GameManager = this;
                    npc.enabled = true;
                }
            }
        }


        // Instantiate NPCS
        foreach (var npc in Game.NPCS.Where(n => n.Scene == Game.Scene.Name))
        {
            npc.CreateInScene();
            Debug.Log(npc.Name+ "Instantiated");
        }
    }

    void HandleTime()
    {
        if (Time.hourOfDay != currentHour)
        {
            currentHour = Time.hourOfDay;
            CheckAllAbsentSchedules();
        }
    }

    public void SaveData(ref PlayerStats data)
    {
        data.NPCS.Clear();
        data.Scenes.Clear();
        data.GlobalFlags.Clear();
        data.NPCS = Game.NPCS;
        data.Scenes = Game.Scenes;
        data.GlobalFlags = Game.GlobalFlags;
        data.Scene = Game.Scene;
    }

    void CheckAllAbsentSchedules(bool forceAll = false)
    {
        List<NPCStateModel> absentNpcs = new List<NPCStateModel>();
        if (forceAll)
        {
            absentNpcs = Game.NPCS;
        }
        else
        {
            absentNpcs = Game.NPCS.Where(npc => npc.Scene != Game.Scene.Name).ToList();
        }

        foreach (var npc in absentNpcs)
        {
            var item = npc.LifetimeSchedule.FirstOrDefault(s => s.Day == Game.dayOfMonth && s.Hour == Time.hourOfDay);
            if (item == null)
            {
                item = npc.WeeklySchedule.FirstOrDefault(s => s.Weekday == Game.WeekDay && s.Hour == Time.hourOfDay);
            }
            if (item != null)
            {
                if (item.Scene == this.Game.Scene.Name)
                {
                    Vector3 instantiatePos;
                    if (!string.IsNullOrEmpty(item.FromExit))
                    {
                        instantiatePos = Game.Scene.Exits.FirstOrDefault(e => e.To == item.FromExit).Position;
                    }
                    else
                    {
                        instantiatePos = Game.Scene.Exits.FirstOrDefault().Position;
                    }
                    npc.Scene = this.Game.Scene.Name;
                    npc.CreateInScene(instantiatePos);
                }
                else
                {
                    npc.Activity = item.Activity;
                    npc.Position = item.Position;
                    npc.Scene = item.Scene;
                }
            }
        }
    }

}
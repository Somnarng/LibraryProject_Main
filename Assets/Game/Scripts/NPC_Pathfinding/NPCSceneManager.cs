using MoreMountains.TopDownEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCSceneManager : Singleton<NPCSceneManager>, IDataPersistence
{

    public PlayerStats Game;
    public TimeManager Time;
    public DefaultGame DGame;
    public NPCList NPCList;

    int currentHour = -1;

    // Use this for initialization

    void Start()
    {
        Time = TimeManager.Instance; //sets timemanager to active time manager

        NPCList = GetComponent<NPCList>(); //gets npc list from gameobject

        foreach (NPC npc in NPCList.npcs) {
            Game.NPCS.Add(new NPCStateModel //adds npc info to Game.NPCS
            {
                Name = npc.name,
                Prefab = npc.prefab,
                Position = new Vector3(3, 3, 3),
                WeeklySchedule = npc.schedule.ToList<ScheduleItem>(),
                LifetimeSchedule = new List<ScheduleItem>(),
                Scene = "TestScene_AI"
            });
        }

        SceneModel TestScene = new SceneModel();
        TestScene.Name = "TestScene_AI"; //setup scene models for checking npc state
        TestScene.Exits = new List<SceneExitModel>
        {
            new SceneExitModel
            {
                Position = new Vector3(11, 6.36000013f, 0),
                To = "TestScene_AI2"
            }
        };
        SceneModel TestScene2 = new SceneModel();
        TestScene2.Name = "TestScene_AI2";
        TestScene2.Exits = new List<SceneExitModel>
        {
            new SceneExitModel
            {
                Position = new Vector3(11, 6.36000013f, 0),
                To = "TestScene_AI"
            }
        };

        Game.Scenes.Add(TestScene);
        Game.Scenes.Add(TestScene2);
        Game.Scene = TestScene;
    }

    private void Update()
    {
        HandleTime();
    }

    public void LoadData(PlayerStats data)
    {
        Game = data;
        //Set all scheduled task items for those currently moving to their place
        if (Game.NPCS == null) { Debug.Log("NO NPCS!"); return; }
        foreach (var npc in Game.NPCS) //loads npcs if their scene is equal to the current scene name
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
        data.NPCS = Game.NPCS;
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
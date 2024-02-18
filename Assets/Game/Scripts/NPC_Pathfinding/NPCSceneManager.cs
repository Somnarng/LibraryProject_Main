using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-999)]
public class NPCSceneManager : Singleton<NPCSceneManager>, IDataPersistence
{

    public PlayerStats Game;
    public TimeManager Time;
    public NPCList NPCList;

    TimeManager.TimeSlot currentHour;

    // Use this for initialization (disable Game = Data in load)

    protected override void OnEnableCallback()
    {
        Time = TimeManager.Instance; //sets timemanager to active time manager

        NPCList = GetComponent<NPCList>(); //gets npc list from gameobject

        foreach (NPCStateModel npc in NPCList.npcs) //searches through npc list, adds any missing npcs to game managers list
        {
            foreach (NPCStateModel n in Game.NPCS) { if (npc.Name == n.Name) return; }
            Game.NPCS.Add(npc);
            Debug.Log("npc added");
        }

        SceneModel TestScene = new SceneModel();
        TestScene.Name = "TestScene_AI"; //setup scene models for checking npc state
        SceneModel TestScene2 = new SceneModel();
        TestScene2.Name = "TestScene_AI2";

        Game.Scenes.Add(TestScene);
        Game.Scenes.Add(TestScene2);
        Game.Scene = TestScene;
        Debug.Log(TestScene);
    }

    private void Update()
    {
        HandleTime();
    }

    public void LoadData(PlayerStats data)
    {
        //Game = data;

        //Set all scheduled task items for NPCS
        if (Game.NPCS == null || Game.NPCS.Count == 0) { Debug.Log("No NPCS!"); return; } //on game load or scene change, check for NPCS
        foreach (var npc in Game.NPCS) //for each npc in the npc list, if the scene is the same as the active scene, check their schedule.
        {
            if (npc.Scene == Game.Scene.Name)
            {
                var item = npc.LifetimeSchedule.FirstOrDefault(s => s.Day == Game.dayOfMonth && s.Slot == Time.currentTimeSlot && s.Month == Game.monthOfYear); //look at lifetime first, then check weekly
                if (item == null)
                {
                    item = npc.WeeklySchedule.FirstOrDefault(s => s.Weekday == Game.WeekDay && s.Slot == Time.currentTimeSlot);
                }
                if (item != null) //if their schedule has an entry that matches the weekday and current time slot, spawn them at the last recorded position on their routine.
                {
                    npc.Position = item.Routine[npc.routinePosition].Position;
                    npc.Scene = item.Scene;
                }
            }
        }

        Game.Scene = Game.Scenes.Where(s => s.Name == SceneManager.GetActiveScene().name).FirstOrDefault();
        OnSceneLoaded();
    }

    
    private void OnSceneLoaded()
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
            Debug.Log(npc.Name + "Instantiated");
        }
    }

    void HandleTime()
    {
        if (Time.currentTimeSlot != currentHour)
        {
            currentHour = Time.currentTimeSlot;
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
            var item = npc.LifetimeSchedule.FirstOrDefault(s => s.Day == Game.dayOfMonth && s.Slot == Time.currentTimeSlot);
            if (item == null)
            {
                item = npc.WeeklySchedule.FirstOrDefault(s => s.Weekday == Game.WeekDay && s.Slot == Time.currentTimeSlot);
            }
            if (item != null)
            {
                if (item.Scene == Game.Scene.Name)
                {
                    npc.Scene = Game.Scene.Name;
                    npc.CreateInScene(item.Routine[0].Position); //spawn the npc at the position of the first item in their routine list
                }
                else
                {
                    npc.Scene = item.Scene;
                }
            }
        }
    }

}
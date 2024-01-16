using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NPCStateModel
{
    public string Name { get; set; }
    public Vector3 Position { get; set; }
    public List<GameFlag> Flags { get; set; }
    public string Scene { get; set; }
    public string Activity { get; set; }

    public List<ScheduleItem> WeeklySchedule { get; set; }
    public List<ScheduleItem> LifetimeSchedule { get; set; }
    // model
    // animator
    public void CreateInScene(Vector3 instantiatePos)
    {
        GameObject prefab = (GameObject)Resources.Load("Game/Prefabs/NPC");
        var instance = (GameObject)UnityEngine.Object.Instantiate(prefab, instantiatePos, Quaternion.identity);
        var npcInstanceState = instance.GetComponent<NPC_Manager>();
        npcInstanceState.State = this;
        npcInstanceState.GameManager = UnityEngine.GameObject.FindObjectOfType<NPCSceneManager>();
        npcInstanceState.enabled = true;
    }
    public void CreateInScene()
    {
        GameObject prefab = (GameObject)Resources.Load("Game/Prefabs/NPC");
        var instance = (GameObject)UnityEngine.Object.Instantiate(prefab, this.Position, Quaternion.identity);
        var npcInstanceState = instance.GetComponent<NPC_Manager>();
        npcInstanceState.State = this;
        npcInstanceState.TeleportToScheduleItem = true;
        npcInstanceState.GameManager = UnityEngine.GameObject.FindObjectOfType<NPCSceneManager>();
        npcInstanceState.enabled = true;
    }
}
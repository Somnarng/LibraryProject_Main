using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class NPCStateModel
{
    public string Name;
    public GameObject Prefab {  get; set; }
    public Vector3 Position { get; set; }
    public List<GameFlag> Flags { get; set; }
    public string Scene;
    public string Activity { get; set; }

    public List<ScheduleItem> WeeklySchedule { get; set; }
    public List<ScheduleItem> LifetimeSchedule { get; set; }
    // model
    // animator
    public void CreateInScene(Vector3 instantiatePos)
    {
        var instance = (GameObject)UnityEngine.Object.Instantiate(Prefab, instantiatePos, Quaternion.identity);
        var npcInstanceState = instance.GetComponent<NPC_Manager>();
        npcInstanceState.State = this;
        npcInstanceState.GameManager = UnityEngine.GameObject.FindObjectOfType<NPCSceneManager>();
        npcInstanceState.enabled = true;
    }
    public void CreateInScene()
    {
        var instance = (GameObject)UnityEngine.Object.Instantiate(Prefab, this.Position, Quaternion.identity);
        var npcInstanceState = instance.GetComponent<NPC_Manager>();
        npcInstanceState.State = this;
        npcInstanceState.TeleportToScheduleItem = true;
        npcInstanceState.GameManager = UnityEngine.GameObject.FindObjectOfType<NPCSceneManager>();
        npcInstanceState.enabled = true;
    }
}
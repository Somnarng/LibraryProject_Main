using System.Collections.Generic;
using UnityEngine;
public class NPCList : MonoBehaviour
{
    public List<NPC> npcs;
}
[System.Serializable]
public class NPC
{
    public string name;
    public GameObject prefab;
    public ScheduleItem[] schedule;
}
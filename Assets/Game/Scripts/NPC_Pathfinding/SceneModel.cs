using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SceneModel {
	public string Name;
	public float NPCRoutineTimer;
	public List<GameFlag> Flags;

	public SceneModel(){
		Flags = new List<GameFlag>();
	}
}
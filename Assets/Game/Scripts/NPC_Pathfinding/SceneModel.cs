using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SceneModel {
	public string Name;
	public List<GameFlag> Flags;
	public List<SceneExitModel> Exits;

	public SceneModel(){
		Flags = new List<GameFlag>();
	}
}
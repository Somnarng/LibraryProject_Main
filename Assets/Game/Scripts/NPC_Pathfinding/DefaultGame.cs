using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class DefaultGame  {

	// Use this for initialization
	public PlayerStats Game;

	public DefaultGame(PlayerStats game){ //used for debugging
		Game = game;
        // ----  Scenes
        SceneModel TestScene = new SceneModel();
		TestScene.Name = "TestScene_AI";
		TestScene.Exits = new List<SceneExitModel>();
		TestScene.Exits.Add (new SceneExitModel{
			Position = new Vector3(11, 6.36000013f, 0),
			To = "TestScene_AI2"
        });
        SceneModel TestScene2 = new SceneModel();
        TestScene2.Name = "TestScene_AI2";
        TestScene2.Exits = new List<SceneExitModel>();
        TestScene2.Exits.Add(new SceneExitModel
        {
            Position = new Vector3(11, 6.36000013f, 0),
            To = "TestScene_AI"
        });

        // ---- NPCs
        List<ScheduleItem> sch = new List<ScheduleItem>();
		sch.Add(new ScheduleItem{
			Day = 0,
			Weekday = 0,
			Hour = 0,
			Activity = "Stand",
			Scene = "TestScene_AI",
			Position = new Vector3(7,2,-12)
		});
		sch.Add(new ScheduleItem{
			Day = 0,
			Weekday = 0,
			Hour = 1,
			Activity = "Stand",
			Scene = "TestScene_AI",
			Position = new Vector3(-3,7,0)
		});
		sch.Add (new ScheduleItem{
			Day = 0,
			Weekday = 0,
			Hour = 2,
			Activity = "Stand",
			Scene = "TestScene_AI2",
			Position = new Vector3(1,1.6f,1)
		});
		sch.Add (new ScheduleItem{
			Day = 0,
			Weekday = 0,
			Hour = 3,
			Activity = "Stand",
			Scene = "TestScene_AI",
			Position = new Vector3(1,1.6f,1),
			FromExit = "TestScene_AI2"
        });

		// ----// ---- NPC Guy
		Game.NPCs.Add (new NPCStateModel{
			Name = "Guy",
			Position = new Vector3(3,3,3),
			WeeklySchedule = sch,
			LifetimeSchedule = new List<ScheduleItem>(),
			Scene = "TestScene_AI"
        });

		// ---- Game State Intialization
		Game.Scenes.Add(TestScene);
		Game.Scenes.Add(TestScene2);
		Game.Scene = TestScene;

	}
}




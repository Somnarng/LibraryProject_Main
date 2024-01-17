using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public string playerName;

    public float timeOfTheDay;
    public int dayOfMonth;
    public int WeekDay
    { get { return dayOfMonth % 7; } }
    public int monthOfYear;
    public int year;

    public List<BookScript> UnsortedBooks;
    public List<BookScript> BooksInInventory;
    public List<ShelfData> ShelfData;

    public List<GameFlag> GlobalFlags;
    public List<SceneModel> Scenes;
    public List<NPCStateModel> NPCS;
    public SceneModel Scene;

    public PlayerStats()
    { //sets default values
        this.playerName = "Default";

        this.timeOfTheDay = 45;
        this.dayOfMonth = 1;
        this.monthOfYear = 1;
        this.year = 1;

        this.UnsortedBooks = new List<BookScript>();
        this.BooksInInventory = new List<BookScript>();
        this.ShelfData = new List<ShelfData>();

        this.GlobalFlags = new List<GameFlag>();
        this.Scenes = new List<SceneModel>();
        this.NPCS = new List<NPCStateModel>();
        this.Scene = new SceneModel();
    }
}
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
    public int monthOfYear;
    public int year;

    public PlayerStats()
    { //sets default values
        this.playerName = "Default";
        this.timeOfTheDay = 45;
        this.dayOfMonth = 1;
        this.monthOfYear = 1;
        this.year = 1;
    }
}
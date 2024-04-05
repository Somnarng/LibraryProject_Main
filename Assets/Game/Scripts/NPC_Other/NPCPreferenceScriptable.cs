using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script should hold the NPC's base & current preferences, as well as have methods to periodically update the latter based on the former
[CreateAssetMenu]
public class NPCPreferenceScriptable : ScriptableObject
{
    [SerializeField] int baseMysteryPref; // assign each genre a preference from 0 to 5 (least to most liked)
    [SerializeField] int baseSciFiPref;
    [SerializeField] int baseHisFicPref;
    [SerializeField] int baseNonFicPref;
    [SerializeField] int baseRomancePref;
    [SerializeField] int baseFantasyPref;
    [SerializeField] int baseHistoryPref;
    [SerializeField] int baseDystopianPref;
    [SerializeField] int baseCrimeFicPref;
    [SerializeField] int baseComedyPref;
    [SerializeField] int baseHorrorPref;
    [SerializeField] int baseWesternPref;
    [SerializeField] int baseThrillerPref;
    [SerializeField] int baseActionPref;
    [SerializeField] int baseFablePref;

    public List<int> runningGenrePrefs;
    private List<int> basePrefsList;


    // Start is called before the first frame update
    void Start()
    {
        //add base preferences to a easily accessible list
        basePrefsList.Add(baseMysteryPref);
        basePrefsList.Add(baseSciFiPref);
        basePrefsList.Add(baseHisFicPref);
        basePrefsList.Add(baseNonFicPref);
        basePrefsList.Add(baseRomancePref);
        basePrefsList.Add(baseFantasyPref);
        basePrefsList.Add(baseHistoryPref);
        basePrefsList.Add(baseDystopianPref);
        basePrefsList.Add(baseCrimeFicPref);
        basePrefsList.Add(baseComedyPref);
        basePrefsList.Add(baseHorrorPref);
        basePrefsList.Add(baseWesternPref);
        basePrefsList.Add(baseThrillerPref);
        basePrefsList.Add(baseActionPref);
        basePrefsList.Add(baseFablePref);

        foreach (int a in basePrefsList) // create new list for current preferences
        {
            runningGenrePrefs.Add(a);
        }
    }

    public void RecalculatePrefs()//randomizes current prefs based on base prefs from -2 to +2 
    {
        for (int b = 0; b < runningGenrePrefs.Count; b++)
        {
            int c = Random.Range(-2, 3); //get random number to fluctuate
            runningGenrePrefs[b] = basePrefsList[b] + c; //modify number

            if (runningGenrePrefs[b] < 0) //make sure number falls within range
            {
                runningGenrePrefs[b] = 0;
            }
            else if (runningGenrePrefs[b] > 5)
            {
                runningGenrePrefs[b] = 5;
            }
        }
    }

    public int CheckGenrePrefs(string genreName) //returns the NPC's current favorability for given genre
    {
        switch (genreName)
        {
            case "Mystery":
                return runningGenrePrefs[0];
            case "SciFi":
                return runningGenrePrefs[1];
            case "Historical Fiction":
                return runningGenrePrefs[2];
            case "NonFiction":
                return runningGenrePrefs[3];
            case "Romance":
                return runningGenrePrefs[4];
            case "Fantasy":
                return runningGenrePrefs[5];
            case "History":
                return runningGenrePrefs[6];
            case "Dystopian Fiction":
                return runningGenrePrefs[7];
            case "Crime Fiction":
                return runningGenrePrefs[8];
            case "Comedy":
                return runningGenrePrefs[9];
            case "Horror":
                return runningGenrePrefs[10];
            case "Western":
                return runningGenrePrefs[11];
            case "Thriller":
                return runningGenrePrefs[12];
            case "Action":
                return runningGenrePrefs[13];
            case "Fable":
                return runningGenrePrefs[14];
            default:
                Debug.Log("Issue in checking Genre Preferences! Genre: " + genreName);
                return -1;
        }
    }

}


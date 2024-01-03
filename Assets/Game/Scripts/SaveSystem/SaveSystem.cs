using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem
{
    private static SaveData s_CurrentData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public TimeManagerSaveData TimeSaveData;
        public SaveData[] ScenesData;
    }

    public static void Save()
    {
        TimeManager.Instance.Save(ref s_CurrentData.TimeSaveData);

        string savefile = Application.persistentDataPath + "/save.sav";
        File.WriteAllText(savefile, JsonUtility.ToJson(s_CurrentData));
    }

    public static void Load()
    {
        string savefile = Application.persistentDataPath + "/save.sav";
        string content = File.ReadAllText(savefile);

        s_CurrentData = JsonUtility.FromJson<SaveData>(content);

        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    static void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TimeManager.Instance.Load(s_CurrentData.TimeSaveData);

        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
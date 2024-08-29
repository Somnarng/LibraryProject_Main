using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{
    public void SaveData()
    {
        DataPersistenceManager.GetRuntimeInstance().SaveGame();
    }
    public void LoadData()
    {
        DataPersistenceManager.GetRuntimeInstance().LoadGame();
    } 
    public void NewData()
    {
        DataPersistenceManager.GetRuntimeInstance().NewGame();
    }
}
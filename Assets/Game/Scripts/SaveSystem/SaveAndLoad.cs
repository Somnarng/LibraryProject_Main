using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{
    public void SaveData()
    {
        DataPersistenceManager.Instance.SaveGame();
    }
    public void LoadData()
    {
        DataPersistenceManager.Instance.LoadGame();
    } 
    public void NewData()
    {
        DataPersistenceManager.Instance.NewGame();
        DataPersistenceManager.Instance.LoadGame();
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private PlayerStats playerStats;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    protected override void OnEnableCallback()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        playerStats = new PlayerStats();
        dataHandler.Save(playerStats);
        LoadGame();
    }
    public void LoadGame()
    {
        //load from file using data handler
        playerStats = dataHandler.Load();
        //if no data, start new game
        if (playerStats == null)
        {
            Debug.Log("Stats not found, starting new game");
            NewGame();
        }
        //push loaded data to managers
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(playerStats);
            Debug.Log("Loaded Data!");
        }
    }
    public void SaveGame()
    {
        //passes data to other scripts, returns updated data
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref playerStats);
            Debug.Log("Saved Data!");
        }

        //saves updated data to file
        dataHandler.Save(playerStats);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

}

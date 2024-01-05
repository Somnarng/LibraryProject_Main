using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "LibraryOhLibrary";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public PlayerStats Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        PlayerStats loadedStats = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //load data from file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //decrypt if using encryption
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                //deserialize
                loadedStats = JsonUtility.FromJson<PlayerStats>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error writing data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedStats;
    }

    public void Save(PlayerStats playerStats)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //create directory for save
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize into json
            string dataToStore = JsonUtility.ToJson(playerStats, true);

            //if using encryption
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //write to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error writing data to file: " + fullPath + "\n" + e);
        }
    }
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}

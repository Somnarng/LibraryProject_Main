using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(PlayerStats data);
    void SaveData(ref PlayerStats data);
}
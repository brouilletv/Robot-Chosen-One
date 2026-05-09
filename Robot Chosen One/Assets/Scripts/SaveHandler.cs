using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public static class SaveSystem
{
    public const string saveFile = "/saveData.json";


    public static void SaveGame(PlayerMovement playerMovementInstance, Respawn respawnInstance, HealthHeartBarV2 healthHeartBarV2Instance)
    {
        string filePath = Application.persistentDataPath + saveFile;
        PlayerMovementData playerMovementData = new PlayerMovementData(playerMovementInstance);
        RespawnData respawnData = new RespawnData(respawnInstance);
        HealthHeartBarV2Data healthHeartBarV2Data = new HealthHeartBarV2Data(healthHeartBarV2Instance);

        DataContainer dataContainer = new DataContainer(playerMovementData, respawnData, healthHeartBarV2Data);

        string dataText = JsonUtility.ToJson(dataContainer, true);
        Debug.Log("SavedGame with the following values:\n" + dataText);
        File.WriteAllText(filePath, dataText);
    }
}


public static class LoadSystem
{
    public static DataContainer LoadGame()
    {
        try
        {
            string filePath = Application.persistentDataPath + SaveSystem.saveFile;
            string fileContent = File.ReadAllText(filePath);
            DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(fileContent);
            return dataContainer;
        }
        catch
        {
            return null;
        }
    }
}


[System.Serializable]
public class DataContainer
{
    [SerializeField] public PlayerMovementData playerMovementData;
    [SerializeField] public RespawnData respawnData;
    [SerializeField] public HealthHeartBarV2Data healthHeartBarV2Data;

    public DataContainer(PlayerMovementData playerMovementData, RespawnData respawnData, HealthHeartBarV2Data healthHeartBarV2Data)
    {
        this.playerMovementData = playerMovementData;
        this.respawnData = respawnData;
        this.healthHeartBarV2Data = healthHeartBarV2Data;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public static class SaveSystem
{
   public const string saveFile = "/saveData.json";


   public static void SaveGame()
   {
      string filePath = Application.persistentDataPath + saveFile;
      PlayerMovementData playerMovementData = new PlayerMovementData(PlayerMovement.instance);

      DataContainer dataContainer = new DataContainer(playerMovementData);

      string dataText = JsonUtility.ToJson(dataContainer, true);
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
    [SerializeField] public RespawnData RespawnData;


    public DataContainer(PlayerMovementData playerMovementData)
    {
        this.playerMovementData = playerMovementData;
    }


    public DataContainer(RespawnData RespawnData)
    {
        this.RespawnData = RespawnData;
    }
}
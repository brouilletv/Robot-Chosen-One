using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Save
{
   public const string saveFile = "/saveData.json";


   public static void SaveGame()
   {
      string filePath = Application.persistentDataPath + saveFile;
      PlayerMovementData playerMovementData = new PlayerMovementData(PlayerMovement.instance);

      DataContainer dataContainer = new DataContainer(playerMovementData);

      string dataText = JsonUtility.ToJson(dataContainer);
      File.WriteAllText(filePath, dataText);
   }
}


[System.Serializable]
public class DataContainer
{
   [SerializeField] PlayerMovementData playerMovementData;

   public DataContainer(PlayerMovementData playerMovementData)
   {
      this.playerMovementData = playerMovementData;
   }
}


public static class Load
{
   public static DataContainer LoadData()
   {
      try
      {
         string filePath = Application.persistentDataPath + Save.saveFile;
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
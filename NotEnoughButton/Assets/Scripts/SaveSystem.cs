using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    private SaveData[] savedData;
    private int maxSaveGameData;
    private bool firstSceneStarted = false;

    public SaveData GetTmpSaveData()
    {
        return GetSaveDataWithIndex(maxSaveGameData);
    }

    public SaveData GetSaveDataWithIndex(int index)
    {
        if (!IsGameDataIndexValid(index)) return null;
        return savedData[index];
    }

    public void Init(int maxSave)
    {
        maxSaveGameData = maxSave;
        savedData = new SaveData[maxSaveGameData + 1];//the last one will be the tmp
        for (int i = 0; i < maxSaveGameData; i++)
        {
            savedData[i] = LoadGameWithIndex(i);
        }

        savedData[maxSaveGameData] = LoadGameTmp();
    }

    private void SavedDataCheck()
    {
        if (savedData.Length == maxSaveGameData + 1) return;
        Debug.Log("save data array lost");
        Init(maxSaveGameData);
    }

    private bool IsGameDataIndexValid(int gameDataIndex)
    {
        SavedDataCheck();
        if (gameDataIndex < 0) return false;
        if (gameDataIndex > maxSaveGameData) return false;
        return true;
    }

    //Save
    public void SaveGameTmp(GameData gameData, LevelManager levelManager)
    {
        if (!firstSceneStarted)
        {
            firstSceneStarted = true;
            return;
        }
        SavedDataCheck();
        string path = Application.persistentDataPath + "/GameDataTmp.neb";
        savedData[maxSaveGameData] = new SaveData();
        savedData[maxSaveGameData].Save(gameData, levelManager);
        SaveGamePath(path, savedData[maxSaveGameData]);
    }

    public void SaveGameWithIndex(int saveFileIndex, GameData gameData, LevelManager levelManager)
    {
        if (!IsGameDataIndexValid(saveFileIndex)) return;
        if (saveFileIndex == maxSaveGameData)
        {
            SaveGameTmp(gameData, levelManager);
            return;
        }
        string path = Application.persistentDataPath + "/GameData" + saveFileIndex + ".neb";
        savedData[saveFileIndex] = new SaveData();
        savedData[saveFileIndex].Save(gameData, levelManager);
        SaveGamePath(path, savedData[saveFileIndex]);
    }

    private void SaveGamePath(string path, SaveData saveData)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        FileStream fileStream = new FileStream(path, FileMode.Create);

        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
    }


    //Load
    public SaveData LoadGameTmp()
    {
        string path = Application.persistentDataPath + "/GameDataTmp.neb";
        return LoadGamePath(path);
    }

    public SaveData LoadGameWithIndex(int saveFileIndex)
    {
        if (!IsGameDataIndexValid(saveFileIndex)) return null;
        if (saveFileIndex >= maxSaveGameData)
        {
            return LoadGameTmp();
        }
        string path = Application.persistentDataPath + "/GameData" + saveFileIndex + ".neb";
        return LoadGamePath(path);
    }

    private SaveData LoadGamePath(string path)
    {
        if (!File.Exists(path))
        {
            //Debug.Log("File doesnt exists: " + path);
            return null;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Open);

        SaveData saveData = binaryFormatter.Deserialize(fileStream) as SaveData;
        
        fileStream.Close();

        return saveData;
    }
}

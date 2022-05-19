using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private LevelManager levelManager;
    private GameData gameData;
    private SaveLoadManager saveLoadManager;

    public bool openAllLevel;
    public bool stopEverything;//stop the MoveControll

    private Vector3 playerPosition;

    // Start is called before the first frame update
    private void Awake()
    {
        if (GameManager.instance != null)//if there is already a GameManager
        {
            Destroy(gameObject);//Destroy the new GameManager that being duplicated
            return;
        }
        instance = this;//To make all the instance being call become this
        DontDestroyOnLoad(gameObject);//prevent this gameObject (GameManager) from being deleted when load a new scene

        saveLoadManager = GameObject.FindObjectOfType<SaveLoadManager>();
        saveLoadManager.Init();

        levelManager = GetComponent<LevelManager>();
        gameData = GetComponent<GameData>();

        levelManager.StartLevelManager();
        gameData.StartGameData();

        if (openAllLevel) levelManager.OpenAllLevel();

        //LoadMainMenuLevel();
    }

    //Level Manager
    public string GetBaseLevelName(int baseLevelNameIndex)
    {
        return levelManager.GetBaseLevelName(baseLevelNameIndex);
    }
    public string GetCurLevelName()
    {
        return levelManager.GetCurLevelName();
    }
    public bool CurLevelIsBaseLevel()
    {
        return levelManager.CurLevelIsBaseLevel();
    }

    public bool CurLevelIsMapLevel()
    {
        return levelManager.CurLevelIsMapLevel();
    }

    public bool LevelIsExist(string levelName)
    {
        return levelManager.LevelIsExist(levelName);
    }
    public bool LevelIsPlayable(string levelName)
    {
        return levelManager.LevelIsPlayable(levelName);
    }
    public bool LevelIsWon(string levelName)
    {
        return levelManager.LevelIsWon(levelName);
    }

    public void SaveLevel()
    {
        levelManager.SaveLevel();
    }
    public void LoadLevel(string levelName)
    {
        levelManager.LoadLevel(levelName);
    }
    
    public void ResetLevel()
    {
        levelManager.ResetLevel();
    }
    public void LoadMainMenuLevel()
    {
        levelManager.LoadMainMenuLevel();
    }
    public void LoadMapLevel()
    {
        levelManager.LoadMapLevel();
    }
    public void NewGame()
    {
        NewGameData();
        levelManager.LoadMapLevel();
    }
    public void NewLevelAdd(string newLevel)
    {
        levelManager.NewLevelAdd(newLevel);
    }

    //Game Data
    public void NewGameData()
    {
        gameData.NewGameData();
    }

    public int GetLevelPoint(string levelName)
    {
        return gameData.GetLevelPoint(levelName);
    }
    public void SetLevelPoint(string levelName, int newLevelPoint)
    {
        gameData.SetLevelPoint(levelName, newLevelPoint);
    }
    public int GetLevelPointTotal()
    {
        return gameData.levelPointTotal;
    }

    public int GetHeartPoint(string levelName)
    {
        return gameData.GetHeartPoint(levelName);
    }
    public void SetHeartPoint(string levelName, int newHeartPoint)
    {
        gameData.SetHeartPoint(levelName, newHeartPoint);
    }
    public int GetHeartPointTotal()
    {
        return gameData.heartPointTotal;
    }

    public int GetGoldPoint(string levelName)
    {
        return gameData.GetGoldPoint(levelName);
    }
    public void SetGoldPoint(string levelName, int newGoldPoint)
    {
        gameData.SetGoldPoint(levelName, newGoldPoint);
    }
    public int GetGoldPointTotal()
    {
        return gameData.goldPointTotal;
    }

    public Vector3 PlayerMapPosition
    {
        get => gameData.PlayerMapPosition;
        set => gameData.PlayerMapPosition = value;
    }

    public void PlayerMapPositionUpdate()
    {
        gameData.PlayerMapPositionUpdate();
    }

    //Save Load Manager
    public void SaveMode()
    {
        saveLoadManager.SaveMode();
    }

    public void LoadMode()
    {
        saveLoadManager.LoadMode();
    }

    //Save System
    public void SaveGameWithIndex(int saveFileIndex)
    {
        saveLoadManager.SaveGameWithIndex(saveFileIndex, gameData, levelManager);
    }
    public void SaveGameTmp()
    {
        saveLoadManager.SaveGameTmp(gameData, levelManager);
    }
    
    public void LoadGameWithIndex(int saveFileIndex)
    {
        SaveData saveData = saveLoadManager.LoadGameWithIndex(saveFileIndex);
        if (saveData == null)
        {
            Debug.Log("saveData" + saveFileIndex + " null");
        }
        saveData.Load();
        LoadMapLevel();
    }
    public void LoadGameTmp()
    {
        SaveData saveData = saveLoadManager.LoadGameTmp();
        if (saveData == null)
        {
            Debug.Log("saveDataTmp null");
        }
        saveData.Load();
        LoadMapLevel();
    }
}

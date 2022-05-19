using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private LevelManager levelManager;
    private string[] scenesInBuild;

    private int[] levelPoint;//save the main point type of
    public int levelPointTotal;
    private int[] heartPoint;
    public int heartPointTotal;
    private int[] goldPoint;
    public int goldPointTotal;

    private Vector3 playerMapPosition;

    public void StartGameData()
    {
        levelManager = GetComponent<LevelManager>();
        NewGameData();
    }

    public void OpenAllLevel()
    {
        for (int i = 1; i < levelPoint.Length; i++)
        {
            if (levelPoint[i] < 0) levelPoint[i] = 0;
        }
    }

    public void NewGameData()
    {
        int levelCount = levelManager.LevelTotalCount();

        levelPoint = new int[levelCount];
        levelPointTotal = 0;
        heartPoint = new int[levelCount];
        heartPointTotal = 0;
        goldPoint = new int[levelCount];
        goldPointTotal = 0;

        for (int i = levelManager.GetLevelIndex("LV1"); i < levelCount; i++)
        {
            levelPoint[i] = heartPoint[i] = goldPoint[i] = -1;
        }
        levelPoint[levelManager.GetLevelIndex("LV1")] = 0;//Open lv1
        PlayerMapPosition = Vector3.zero;
    }


    public int GetLevelPoint(string levelName)
    {
        if (!levelManager.LevelIsExist(levelName)) return -2;
        return levelPoint[levelManager.GetLevelIndex(levelName)];
    }

    public void SetLevelPoint(string levelName, int newLevelPoint)
    {
        if (!levelManager.LevelIsExist(levelName)) return;
        /*if (!levelManager.LevelIsPlayable(levelName) && newLevelPoint > 0)//incase game just want to open the level
        {
            return;
        }*///change this into comment for loadgame to work well, may have to change it back if sth wrong happen
        int levelIndex = levelManager.GetLevelIndex(levelName);
        if (newLevelPoint > levelPoint[levelIndex])
        {
            if (levelPoint[levelIndex] > 0) levelPointTotal += newLevelPoint - levelPoint[levelIndex];//incase curlvpoint==-1 so the total +1 more
            else levelPointTotal += newLevelPoint;
            Debug.Log(levelPointTotal);
            levelPoint[levelIndex] = newLevelPoint;
        }
    }

    public int GetHeartPoint(string levelName)
    {
        if (!levelManager.LevelIsExist(levelName)) return -2;
        return heartPoint[levelManager.GetLevelIndex(levelName)];
    }


    public void SetHeartPoint(string levelName, int newHeartPoint)
    {
        if (!levelManager.LevelIsPlayable(levelName)) return;
        int levelIndex = levelManager.GetLevelIndex(levelName);
        if (newHeartPoint > heartPoint[levelIndex])
        {
            heartPointTotal += newHeartPoint - heartPoint[levelIndex];
            heartPoint[levelIndex] = newHeartPoint;
        }
    }

    public int GetGoldPoint(string levelName)
    {
        if (!levelManager.LevelIsExist(levelName)) return -2;
        return goldPoint[levelManager.GetLevelIndex(levelName)];
    }

    public void SetGoldPoint(string levelName, int newGoldPoint)
    {
        if (!levelManager.LevelIsPlayable(levelName)) return;
        int levelIndex = levelManager.GetLevelIndex(levelName);
        if (newGoldPoint > goldPoint[levelIndex])
        {
            goldPointTotal += newGoldPoint - goldPoint[levelIndex];
            goldPoint[levelIndex] = newGoldPoint;
        }
    }

    public void PlayerMapPositionUpdate()
    {
        if(levelManager.CurLevelIsMapLevel())
        {
            PlayerMapPosition = GameObject.Find("Player").transform.position;
        }
    }
    public void RealPlayerMapPositionUpdate()
    {
        if(levelManager.CurLevelIsMapLevel())
        {
            GameObject.Find("Player").transform.position = PlayerMapPosition;
        }
    }

    

    public Vector3 PlayerMapPosition
    {
        get => playerMapPosition;
        set => playerMapPosition = value;
    }

    //Save System
    public int[] GetLevelPointArray()
    {
        return levelPoint;
    }
    public int[] GetHeartPointArray()
    {
        return heartPoint;
    }
    public int[] GetGoldPointArray()
    {
        return goldPoint;
    }

}

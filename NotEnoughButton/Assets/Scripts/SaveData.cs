using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string[] scenesInBuild;

    public int levelPointTotal;
    public int[] levelPoint;//save the main point type of
    public int[] heartPoint;
    public int[] goldPoint;

    public DateTime lastSave;

    public float[] playerPositionOnMap;

    public void Save(GameData gameData, LevelManager levelManager)
    {
        scenesInBuild = levelManager.GetScenesInBuild();
        levelPoint = gameData.GetLevelPointArray();
        heartPoint = gameData.GetHeartPointArray();
        goldPoint = gameData.GetGoldPointArray();
        levelPointTotal = gameData.levelPointTotal;

        lastSave = DateTime.Now;
        //Debug.Log(lastSave);

        GameManager.instance.PlayerMapPositionUpdate();
        playerPositionOnMap = new float[3] { gameData.PlayerMapPosition.x, gameData.PlayerMapPosition.y, gameData.PlayerMapPosition.z };
    }

    public void Load()
    {
        GameManager.instance.NewGameData();
        for (int i = 0; i < scenesInBuild.Length; i++)
        {
            GameManager.instance.SetLevelPoint(scenesInBuild[i], levelPoint[i]);
            GameManager.instance.SetHeartPoint(scenesInBuild[i], heartPoint[i]);
            GameManager.instance.SetGoldPoint(scenesInBuild[i], goldPoint[i]);
            Debug.Log(scenesInBuild[i] + ": " + GameManager.instance.GetLevelPoint(scenesInBuild[i]) + " " + DateTime.Now);
        }

        GameManager.instance.PlayerMapPosition = new Vector3(playerPositionOnMap[0], playerPositionOnMap[1], playerPositionOnMap[2]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameData gameData;
    private string curLevelName;
    private List<string> scenesInBuild = new List<string>();
    [SerializeField] private List<string> baseLevelName = new List<string>();
    [SerializeField] private string mapLevelName;
    [SerializeField] private string mainMenuLevelName;

    public void StartLevelManager()
    {
        gameData = GetComponent<GameData>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)//Get the list of scene name
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }
        SceneManager.sceneLoaded += LoadState;
    }

    public void OpenAllLevel()
    {
        curLevelName = mapLevelName;
        gameData.OpenAllLevel();
    }

    public int LevelTotalCount()
    {
        return scenesInBuild.Count;
    }

    public string GetBaseLevelName(int baseLevelNameIndex)
    {
        return baseLevelName[baseLevelNameIndex];
    }

    public string GetCurLevelName()
    {
        return curLevelName;
    }

    public int GetLevelIndex(string levelName)
    {
        return scenesInBuild.IndexOf(levelName);
    }

    public bool CurLevelIsBaseLevel()
    {
        return LevelIsBaseLevel(curLevelName);
    }
    
    public bool LevelIsBaseLevel(string levelName)
    {
        return baseLevelName.Contains(levelName);
    }
    
    public bool CurLevelIsMapLevel()
    {
        return LevelIsMapLevel(curLevelName);
    }
    
    public bool LevelIsMapLevel(string levelName)
    {
        return levelName == mapLevelName;
    }


    public bool LevelIsExist(string levelName)
    {
        if (!scenesInBuild.Contains(levelName)) return false;
        return true;
    }

    public bool LevelIsPlayable(string levelName)
    {
        if (!LevelIsExist(levelName)) return false;//check if the scene name even exist
        if (LevelIsBaseLevel(levelName)) return true;
        if (gameData.GetLevelPoint(levelName) < 0) return false;
        return true;
    }

    public bool LevelIsWon(string levelName)
    {
        if (!LevelIsPlayable(levelName)) return false;
        if (gameData.GetHeartPoint(levelName) < 0) return false;
        return true;
    }

    public void LoadLevel(string levelName)
    {
        if (!LevelIsPlayable(levelName)) return;
        gameData.PlayerMapPositionUpdate();
        //Debug.Log(curLevelName + " " + gameData.PlayerMapPosition);
        curLevelName = levelName;
        SceneManager.LoadScene(levelName);
    }

    public void ResetLevel()
    {
        LoadLevel(curLevelName);
    }

    public void LoadMainMenuLevel()
    {
        LoadLevel(mainMenuLevelName);
    }

    public void LoadMapLevel()
    {
        LoadLevel(mapLevelName);
    }

    public void NewLevelAdd(string newLevel)
    {
        if (!LevelIsPlayable(newLevel))
        {
            gameData.SetLevelPoint(newLevel,0);
        }
    }

    public void SaveLevel()
    {
        /*string s = "";
        if (curLevelName == baseLevelName) playerPosition = GameObject.Find("Player").transform.position;//Only save the position of player in lv0
        //Debug.Log("save state: " + curSceneIndex + " & " + playerPosition);
        s += playerPosition.x.ToString() + "|";
        s += playerPosition.y.ToString() + "|";
        s += playerPosition.z.ToString() + "|";
        for (int i = 0; i < levelPoint.Count; i++)
        {
            s += levelPoint[i].ToString() + "|";
            s += heartPoint[i].ToString() + "|";
            s += goldPoint[i].ToString() + "|";
        }

        PlayerPrefs.SetString("SaveLevel", s);*/


        gameData.PlayerMapPositionUpdate();
    }

    private void LoadState(Scene s, LoadSceneMode mode)
    {
        /*//Debug.Log(s + " & " + mode);
        
        
        if (!PlayerPrefs.HasKey("SaveLevel")) return;

        string[] data = PlayerPrefs.GetString("SaveLevel").Split('|');//split the whole string with '|'


        int point;
        int index = 0;
        gameData.levelPointTotal = 0;
        gameData.heartPointTotal = 0;
        gameData.goldPointTotal = 0;
        for (int i = 3; i < data.Length; i += 3)
        {
            //Debug.Log("String load: " + data[i]);
            if (int.TryParse(data[i], out point))
            {
                levelPoint[index] = point;
                if (point > 0) levelPointTotal += point;

                heartPoint[index] = int.Parse(data[i + 1]);
                if (heartPoint[index] > 0) heartPointTotal += heartPoint[index];

                goldPoint[index] = int.Parse(data[i + 2]);
                if (goldPoint[index] > 0) goldPointTotal += goldPoint[index];

                index++;
            }
        }*/
        AudioManager.instance.DeleteAudioOnNewLevel();
        gameData.RealPlayerMapPositionUpdate();
        GameManager.instance.SaveGameTmp();
        GameManager.instance.stopEverything = false;
    }

    //Save System
    public string[] GetScenesInBuild()
    {
        return scenesInBuild.ToArray();
    }
}

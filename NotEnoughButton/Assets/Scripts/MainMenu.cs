using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.instance.NewGame();
    }

    public void LoadGame()
    {
        GameManager.instance.LoadMode();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveMode();
    }

    public void Setting()
    {
        SettingManager.instance.OpenSetting();
        //Debug.Log("Setting ok");
    }

    public void QuitGame()
    {
        //GameManager.instance.SaveLevel();
        Application.Quit();
    }

    public void MainMenuLevel()
    {
        GameManager.instance.LoadMainMenuLevel();
    }
}

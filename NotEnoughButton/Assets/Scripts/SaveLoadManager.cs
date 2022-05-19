using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : ManagerObject
{
    private int maxSaveLoadSlot;

    private bool saveModeOn;
    private bool loadModeOn;
    private int curSaveDataIndex;

    private SaveSystem saveSystem;

    [SerializeField] private Text modeName;
    [SerializeField] private GameObject saveLoadPanel;
    [SerializeField] private Transform saveDataPanel;
    [SerializeField] private GameObject confirmPanel;


    public override void Init()
    {
        base.Init();
        maxSaveLoadSlot = saveDataPanel.transform.childCount - 1;
        Reset();
        saveSystem = GetComponent<SaveSystem>();
        saveSystem.Init(maxSaveLoadSlot);
    }

    private void Reset()
    {
        saveModeOn = loadModeOn = false;
    }

    public void SaveMode()
    {
        saveModeOn = true;
        modeName.text = "Save Game";
        SaveLoadUIActivate();
    }

    public void LoadMode()
    {
        loadModeOn = true;
        modeName.text = "Load Game";
        SaveLoadUIActivate();
    }

    private void SaveLoadUIActivate()
    {
        if (!saveModeOn && !loadModeOn)
        {
            Quit();
            return;
        }

        saveLoadPanel.SetActive(true);
        //Debug.Log("maxSaveLoadSlot: " + maxSaveLoadSlot);
        for (int i = 0; i <= maxSaveLoadSlot; i++)
        {
            saveDataPanel.GetChild(i).GetComponent<SaveLoadPanel>().Init(saveSystem.GetSaveDataWithIndex(i), i, maxSaveLoadSlot, this);
        }
    }

    public void Quit()
    {
        saveLoadPanel.SetActive(false);
        Reset();
    }

    public void OnSaveLoadPanelClick(int index)
    {
        curSaveDataIndex = index;
        if (saveSystem.GetSaveDataWithIndex(index) == null && loadModeOn) return;
        confirmPanel.SetActive(true);
    }

    public void Confirm(bool yes)
    {
        if (!confirmPanel.activeSelf) return;
        if (yes)
        {
            if (saveModeOn)
            {
                GameManager.instance.SaveGameWithIndex(curSaveDataIndex);
                saveDataPanel.GetChild(curSaveDataIndex).GetComponent<SaveLoadPanel>().Init(saveSystem.GetSaveDataWithIndex(curSaveDataIndex), curSaveDataIndex, maxSaveLoadSlot, this);
            }
            if (loadModeOn)
            {
                GameManager.instance.LoadGameWithIndex(curSaveDataIndex);
                Quit();
            }
        }
        confirmPanel.SetActive(false);
    }

    //Save System
    public void SaveGameWithIndex(int saveFileIndex, GameData gameData, LevelManager levelManager)
    {
        saveSystem.SaveGameWithIndex(saveFileIndex, gameData, levelManager);
    }
    public void SaveGameTmp(GameData gameData, LevelManager levelManager)
    {
        saveSystem.SaveGameTmp(gameData, levelManager);
    }

    public SaveData LoadGameWithIndex(int saveFileIndex)
    {
        return saveSystem.LoadGameWithIndex(saveFileIndex);
    }
    public SaveData LoadGameTmp()
    {
        return saveSystem.LoadGameTmp();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadPanel : MonoBehaviour
{
    private SaveLoadManager saveLoadManager;
    private int saveDataIndex;

    public void Init(SaveData saveData, int index, int maxIndex, SaveLoadManager saveLoad)
    {
        saveDataIndex = index;
        saveLoadManager = saveLoad;
        ShowDetail(saveData, maxIndex);
    }

    private void ShowDetail(SaveData saveData, int maxIndex)
    {
        if (saveDataIndex < maxIndex) transform.GetChild(0).GetComponent<Text>().text = "Save File " + saveDataIndex + ":";
        else transform.GetChild(0).GetComponent<Text>().text = "Auto Save File:";

        if (saveData != null) transform.GetChild(1).GetComponent<Text>().text = "Level Point Total: " + saveData.levelPointTotal + "\nLast Save: " + saveData.lastSave;
        else transform.GetChild(1).GetComponent<Text>().text = "null";
    }

    public void OnClick()
    {
        if (saveLoadManager == null) return;
        saveLoadManager.OnSaveLoadPanelClick(saveDataIndex);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsUI : MonoBehaviour
{
    public Image slotImagePrefab;
    
    private GameObject player;
    private AbleButton ableButton;


    private int maxSlot = 17;
    private List<Image> slotImage;
    private int plusBoxesCount = 0;
    private int buttonBoxesCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            ableButton = player.GetComponentInChildren<AbleButton>();
            if (ableButton != null)
            {
                slotImage = new List<Image>(maxSlot);
                for (int i = 0; i < maxSlot; i++)
                {
                    slotImage.Add(Instantiate(slotImagePrefab, transform));
                    RemoveSlot(slotImage[i]);
                }
            }
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        slotCheck();
    }

    private void slotCheck()
    {
        //plusBox check
        int newPlusBoxesCount = player.transform.childCount;
        if (newPlusBoxesCount > plusBoxesCount)
        {
            for (; plusBoxesCount < newPlusBoxesCount; plusBoxesCount++)
            {
                slotImage[plusBoxesCount].enabled = true;
            }
        }
        else if (newPlusBoxesCount < plusBoxesCount)
        {
            for (; plusBoxesCount > newPlusBoxesCount; plusBoxesCount--)
            {
                RemoveSlot(slotImage[plusBoxesCount - 1]);
            }
        }

        //buttonBox check
        int newButtonBoxCount = ableButton.orderOutPlusBox.Count;
        for (int i = 0; i < newButtonBoxCount; i++)
        {
            int index = ableButton.orderOutPlusBox[i];
            //find player
            if (index == 0) slotImage[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
            else slotImage[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            //set button
            ButtonBox buttonBox = player.transform.GetChild(index).GetComponentInChildren<ButtonBox>();
            ButtonVisualSettup(buttonBox.buttonStruct, slotImage[i].transform.GetChild(0).GetComponent<Image>(), true);
        }
        if (newButtonBoxCount < buttonBoxesCount)
        {
            for (; buttonBoxesCount > newButtonBoxCount; buttonBoxesCount--)
            {
                ButtonVisualSettup(new ButtonStruct(false), slotImage[buttonBoxesCount - 1].transform.GetChild(0).GetComponent<Image>(), false);
            }
        }
        else buttonBoxesCount = newButtonBoxCount;
    }

    private void ButtonVisualSettup(ButtonStruct buttonStruct, Image buttonBoxSlot, bool enableButton)
    {
        buttonBoxSlot.enabled = enableButton;
        //Move
        buttonBoxSlot.transform.GetChild(0).GetComponent<Image>().enabled = buttonStruct.moveLeft;
        buttonBoxSlot.transform.GetChild(1).GetComponent<Image>().enabled = buttonStruct.moveRight;
        buttonBoxSlot.transform.GetChild(2).GetComponent<Image>().enabled = buttonStruct.moveUp;
        buttonBoxSlot.transform.GetChild(3).GetComponent<Image>().enabled = buttonStruct.moveDown;
        //Rotate
        buttonBoxSlot.transform.GetChild(4).GetComponent<Image>().enabled = buttonStruct.rotateLeft;
        buttonBoxSlot.transform.GetChild(5).GetComponent<Image>().enabled = buttonStruct.rotateRight;
        //Undo
        buttonBoxSlot.transform.GetChild(6).GetComponent<Image>().enabled = buttonStruct.undo;

    }

    private void RemoveSlot(Image plusBox)
    {
        plusBox.enabled = false;
        ButtonVisualSettup(new ButtonStruct(false), plusBox.transform.GetChild(0).GetComponent<Image>(), false);
        plusBox.transform.GetChild(1).GetComponent<Image>().enabled = false;
    }
}

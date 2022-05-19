using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBox : Collidable
{
    private bool isAttached = false;

    public ButtonStruct buttonStruct;
    public bool moveLeft, moveRight, moveUp, moveDown, rotateLeft, rotateRight, undo;

    public List<GameObject> buttonVisualPrefab;

    protected override void Start()
    {
        buttonStruct = new ButtonStruct(moveLeft, moveRight, moveUp, moveDown, rotateLeft, rotateRight, undo);
        ButtonVisualSettup();
        base.Start();
        if (transform.parent != null)
        {
            AddToFreePlusBox(transform.GetComponentInParent<PlusBox>());
        }
    }

    private void ButtonVisualSettup()
    {
        GameObject visual;
        //Move
        if (buttonStruct.moveLeft)
        {
            visual = Instantiate(buttonVisualPrefab[0], transform);
            visual.transform.localEulerAngles += new Vector3(0, 0, 180);
        }
        if (buttonStruct.moveRight)
        {
            visual = Instantiate(buttonVisualPrefab[0], transform);
        }
        if (buttonStruct.moveUp)
        {
            visual = Instantiate(buttonVisualPrefab[0], transform);
            visual.transform.localEulerAngles += new Vector3(0, 0, 90);
        }
        if (buttonStruct.moveDown)
        {
            visual = Instantiate(buttonVisualPrefab[0], transform);
            visual.transform.localEulerAngles += new Vector3(0, 0, 270);
        }
        //Rotate
        if (buttonStruct.rotateLeft)
        {
            visual = Instantiate(buttonVisualPrefab[1], transform);
            visual.transform.localEulerAngles += new Vector3(0, 0, 180);
        }
        if (buttonStruct.rotateRight)
        {
            visual = Instantiate(buttonVisualPrefab[1], transform);
        }
        //Undo
        if (buttonStruct.undo)
        {
            visual = Instantiate(buttonVisualPrefab[2], transform);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (transform.eulerAngles != Vector3.zero) 
            transform.eulerAngles = Vector3.zero;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (!isAttached)
        {
            PlusBox opponentPlusBox = coll.GetComponentInParent<PlusBox>();
            float plusBoxColliableDistance = 0.02f;
            if (opponentPlusBox != null)
            {
                if (Vector3.SqrMagnitude(transform.position - opponentPlusBox.transform.position) <= plusBoxColliableDistance)
                {
                    AbleButton opponentAbleButton = opponentPlusBox.GetComponentInParent<AbleButton>();
                    if (opponentAbleButton != null)
                    {
                        if (!opponentAbleButton.isOrderOutProcessing)
                        {
                            opponentAbleButton.isOrderOutProcessing = true;
                            AddToPlusBox(opponentPlusBox);
                            opponentAbleButton.isOrderOutProcessing = false;
                        }
                    }
                    else AddToPlusBox(opponentPlusBox);
                }
            }
        }
    }

    private void AddToPlusBox(PlusBox opponentPlusBox)
    {
        //isAttached = true;
        if (!opponentPlusBox.HasButton())
        {
            AddToFreePlusBox(opponentPlusBox);
        }
        else if (opponentPlusBox.IsAttached())//Has button
        {
            //Debug.Log("HasButton "+Time.time+" " + opponentPlusBox.name);
            AddToHasButtonPlusBox(opponentPlusBox);
        }
    }


    public void AddToFreePlusBox(PlusBox opponentPlusBox)
    {
        if (opponentPlusBox == null)
        {
            Debug.Log("AddToFreePlusBox: null Plus Box");
            return;
        }
        transform.SetParent(opponentPlusBox.transform);
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(0.6f, 0.6f, 0);
        isAttached = true;
        AbleButton ableButton = opponentPlusBox.GetComponentInParent<AbleButton>();
        if (ableButton != null)
        {
            ableButton.AddPlusBoxOrderOut(opponentPlusBox);
        }
        //else Debug.Log("AddToFreePlusBox: albeButton null");
    }

    private void AddToHasButtonPlusBox(PlusBox opponentPlusBox)
    {
        if (opponentPlusBox == null)
        {
            Debug.Log("AddToHasButtonPlusBox: null Plus Box");
            return;
        }
        AbleButton ableButton = opponentPlusBox.GetComponentInParent<AbleButton>();
        if (ableButton != null)
        {
            ableButton.AdjustButtons(this, opponentPlusBox);
        }
        else Debug.Log("AddToHasButtonPlusBox: albeButton null");
    }

    //Keep the same index order in order out, just change the plus box index
    public void ChangeToAnotherPlusBox(PlusBox newPlusBox)
    {
        PlusBox curPlusBox = transform.GetComponentInParent<PlusBox>();//before change plusbox
        if (curPlusBox != null)
        {
            AbleButton ableButton = curPlusBox.GetComponentInParent<AbleButton>();
            if (ableButton != null)
            {
                AddToFreePlusBox(newPlusBox);
                //fix the order out
                int curIndex = ableButton.orderOutPlusBox.IndexOf(ableButton.IndexOfPlusBox(curPlusBox));
                int newPlusBoxIndex = ableButton.IndexOfPlusBox(newPlusBox);
                ableButton.orderOutPlusBox.RemoveAt(ableButton.orderOutPlusBox.LastIndexOf(newPlusBoxIndex));//remove the new number when AddToFree
                ableButton.orderOutPlusBox[curIndex] = newPlusBoxIndex;//set new plus box but for the same index of buttonbox in order out
            }
        }
    }

    public void DetroyButtonBox()
    {
        transform.SetParent(null, true);
        gameObject.SetActive(false);
    }
}

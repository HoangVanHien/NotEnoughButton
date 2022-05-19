using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbleButton : MonoBehaviour
{
    public MoveControll moveControll;
    public List<int> orderOutPlusBox;//Save the index number of PlusBox as child, the delete order when add new button
    public bool isOrderOutProcessing = false;

    private void Start()
    {
        moveControll = GetComponent<MoveControll>();
    }

    /*private void FixedUpdate()
    {
        for (int i = 0; i < orderOutPlusBox.Count; i++)
        {
            Debug.Log(orderOutPlusBox[i] + "/" + orderOutPlusBox.Count + ": " + transform.GetChild(orderOutPlusBox[i]).name);

        }
    }*/

    public PlusBox FindFreePlusBox()//PlusBox doesnt have ButtonBox
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            PlusBox plusBox = transform.GetChild(i).GetComponent<PlusBox>();
            if (!plusBox.HasButton())
                return plusBox;
        }
        return null;
    }

    public int IndexOfPlusBox(PlusBox plusBox)//find the chld index of the plus box
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<PlusBox>() == plusBox) return i;
        }
        return -1;
    }

    public void AddPlusBoxOrderOut(PlusBox plusBox)
    {
        int index = IndexOfPlusBox(plusBox);
        if (index != -1){
            orderOutPlusBox.Add(index);
        }
    }

    public void AdjustButtons(ButtonBox newButton, PlusBox addToThis)//add new button box
    {
        PlusBox freePlusBox = FindFreePlusBox();
        int index = orderOutPlusBox.IndexOf(IndexOfPlusBox(addToThis));//index in of addToThis in orderout
        if (freePlusBox != null)
        {
            addToThis.GetComponentInChildren<ButtonBox>().ChangeToAnotherPlusBox(freePlusBox);
            newButton.AddToFreePlusBox(addToThis);
            //Debug.Log("4 "+ newButton.name+" "+freePlusBox.name+" "+ addToThis.name+" " + orderOutPlusBox.Count);
        }
        else
        {
            ButtonBox curButton = transform.GetChild(orderOutPlusBox[0]).GetComponentInChildren<ButtonBox>();
            if (curButton != null)
            {
                curButton.DetroyButtonBox();
                if (orderOutPlusBox.Count > 1)
                {
                    int nextPlusBoxIndex = orderOutPlusBox[1];//Get the next index from Order out
                    PlusBox curPlusBox;
                    PlusBox changeToThis = transform.GetChild(orderOutPlusBox[0]).GetComponent<PlusBox>();
                    for (int i = 1; i <= index; i++)//move the button in order out
                    {
                        curPlusBox = transform.GetChild(orderOutPlusBox[i]).GetComponent<PlusBox>();
                        curButton = curPlusBox.GetComponentInChildren<ButtonBox>();
                        curButton.ChangeToAnotherPlusBox(changeToThis);
                        changeToThis = curPlusBox;
                        //Debug.Log(orderOutPlusBox[i] + "/" + i + ": " + transform.GetChild(orderOutPlusBox[i]).name);
                    }
                }
                orderOutPlusBox.RemoveAt(0);
                newButton.AddToFreePlusBox(addToThis);
            }
        }
    }

    public void MoveOnPress(Vector3 direction)//havent move
    {
        if (!moveControll.MoveAble()) return;
        //moveControll.SetLastMoveTime();
        Vector3 ableMove = Vector3.zero;
        for (int i = 0; i < orderOutPlusBox.Count; i++)
        {
            PlusBox plusBox = transform.GetChild(orderOutPlusBox[i]).GetComponent<PlusBox>();
            //Debug.Log(plusBox.name);
            if (plusBox.HasButton())
            {
                ButtonBox button = plusBox.GetComponentInChildren<ButtonBox>();
                switch (direction.x)
                {
                    case -1:
                        {
                            if (button.buttonStruct.moveLeft)
                            {
                                ableMove.x += -1;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (button.buttonStruct.moveRight)
                            {
                                ableMove.x += 1;
                            }
                            break;
                        }
                }
                switch (direction.y)
                    {
                        case -1:
                            {
                                if (button.buttonStruct.moveDown)
                                {
                                    ableMove.y += -1;
                                }
                                break;
                            }
                        case 1:
                            {
                                if (button.buttonStruct.moveUp)
                                {
                                    ableMove.y += 1;
                                }
                                break;
                            }
                    }
            }
        }
        if (ableMove == Vector3.zero) return;
        AudioManager.instance.Play("Move");
        moveControll.Move(ableMove);
    }

    public void RotateOnPress(Vector3 angle)
    {
        if (!moveControll.MoveAble()) return;
        //moveControll.SetLastMoveTime();
        Vector3 ableRotate = Vector3.zero;
        for (int i = 0; i < orderOutPlusBox.Count; i++)
        {
            PlusBox plusBox = transform.GetChild(orderOutPlusBox[i]).GetComponent<PlusBox>();
            if (plusBox.HasButton())
            {
                ButtonBox button = plusBox.GetComponentInChildren<ButtonBox>();
                switch (angle.z)
                {
                    case -1:
                        {
                            if (button.buttonStruct.rotateLeft)
                            {
                                ableRotate += angle;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (button.buttonStruct.rotateRight)
                            {
                                ableRotate += angle;
                            }
                            break;
                        }
                }
            }
        }
        if (ableRotate == Vector3.zero) return;
        moveControll.RotateMove(ableRotate);
    }

    public void DestroyPlusBox(PlusBox destroyThisPlusBox)
    {
        if (!moveControll.MoveAble()) return;
        if (destroyThisPlusBox == transform.GetChild(0).GetComponent<PlusBox>())//game over
        {
            GameObject.Find("LevelUI").GetComponent<LevelUI>().GameOver();
        }
        else
        {
            //use BFS to separate not destroy plusbox
            List<PlusBox> notDestroyPlusBox = new List<PlusBox>();
            List<PlusBox> destroyPlusBox = new List<PlusBox>();
            int curPlusBoxDestroyListIndex;
            List<int> destroyPlusBoxIndex = new List<int>();

            destroyPlusBoxIndex.Add(-1);//the first place is to save destroyThisPlusBox index

            notDestroyPlusBox.Add(transform.GetChild(0).GetComponent<PlusBox>());//player
            
            //fill all the plus box in the destroyPlusBox
            for (int i = 1; i < transform.childCount; i++)
            {
                PlusBox curPlusBox = transform.GetChild(i).GetComponent<PlusBox>();
                if (curPlusBox != destroyThisPlusBox)
                {
                    destroyPlusBox.Add(curPlusBox);
                    destroyPlusBoxIndex.Add(i);
                }
                else
                {
                    destroyPlusBoxIndex[0] = i;
                }
            }

            //find attachable plusbox with bfs and add to notDestroyPlusBox
            for (int i = 0; i < notDestroyPlusBox.Count; i++)
            {
                if (destroyPlusBox.Count <= 0)
                {
                    break;
                }

                curPlusBoxDestroyListIndex = destroyPlusBox.FindIndex(pb => pb.transform.localPosition == notDestroyPlusBox[i].transform.localPosition + Vector3.left);

                if (curPlusBoxDestroyListIndex != -1)
                {
                    //move attachable plus box
                    notDestroyPlusBox.Add(destroyPlusBox[curPlusBoxDestroyListIndex]);
                    destroyPlusBox.RemoveAt(curPlusBoxDestroyListIndex);
                    destroyPlusBoxIndex.RemoveAt(curPlusBoxDestroyListIndex + 1);
                }

                curPlusBoxDestroyListIndex = destroyPlusBox.FindIndex(pb => pb.transform.localPosition == notDestroyPlusBox[i].transform.localPosition + Vector3.right);
                if (curPlusBoxDestroyListIndex != -1)
                {
                    //move attachable plus box
                    notDestroyPlusBox.Add(destroyPlusBox[curPlusBoxDestroyListIndex]);
                    destroyPlusBox.RemoveAt(curPlusBoxDestroyListIndex);
                    destroyPlusBoxIndex.RemoveAt(curPlusBoxDestroyListIndex + 1);
                }

                curPlusBoxDestroyListIndex = destroyPlusBox.FindIndex(pb => pb.transform.localPosition == notDestroyPlusBox[i].transform.localPosition + Vector3.up);
                if (curPlusBoxDestroyListIndex != -1)
                {
                    //move attachable plus box
                    notDestroyPlusBox.Add(destroyPlusBox[curPlusBoxDestroyListIndex]);
                    destroyPlusBox.RemoveAt(curPlusBoxDestroyListIndex);
                    destroyPlusBoxIndex.RemoveAt(curPlusBoxDestroyListIndex + 1);
                }

                curPlusBoxDestroyListIndex = destroyPlusBox.FindIndex(pb => pb.transform.localPosition == notDestroyPlusBox[i].transform.localPosition + Vector3.down);
                if (curPlusBoxDestroyListIndex != -1)
                {
                    //move attachable plus box
                    notDestroyPlusBox.Add(destroyPlusBox[curPlusBoxDestroyListIndex]);
                    destroyPlusBox.RemoveAt(curPlusBoxDestroyListIndex);
                    destroyPlusBoxIndex.RemoveAt(curPlusBoxDestroyListIndex + 1);
                }

            }

            //Fix the new child index in orderOutPlusBox
            for (int i = 0; i < orderOutPlusBox.Count; i++)
            {
                int changeIndexInto = orderOutPlusBox[i];
                for (int j = 0; j < destroyPlusBoxIndex.Count; j++)
                {
                    if (orderOutPlusBox[i] > destroyPlusBoxIndex[j])
                    {
                        changeIndexInto--;
                    }
                    else if(orderOutPlusBox[i] == destroyPlusBoxIndex[j])//will no longer attach to the parent
                    {
                        changeIndexInto = -1;
                        break;
                    }
                }
                if (changeIndexInto == -1)
                {
                    orderOutPlusBox.RemoveAt(i);
                    i--;
                }
                else
                {
                    orderOutPlusBox[i] = changeIndexInto;
                }
            }

            destroyThisPlusBox.DetroyPlusBox();
            for (int i = 0; i < destroyPlusBox.Count; i++)
            {
                destroyPlusBox[i].DetachPlusBox();
            }
        }
    }
}

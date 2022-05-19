using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusBox : Collidable
{
    private bool isAttached = false;

    protected override void Start()
    {
        if (transform.parent != null)
        {
            isAttached = true;
        }
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (transform.eulerAngles != Vector3.zero)
            transform.eulerAngles = Vector3.zero;
    }

    public bool IsAttached()
    {
        return isAttached;
    }

    public bool HasButton()
    {
        return (GetComponentInChildren<ButtonBox>() != null);
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (!isAttached)
        {
            PlusBox opponentPlusBox = coll.GetComponentInParent<PlusBox>();
            if (opponentPlusBox != null)
            {
                if (opponentPlusBox.IsAttached())
                {
                    Transform opponentParent = opponentPlusBox.transform.parent.transform;
                    AbleButton opponentAbleButton = opponentParent.GetComponent<AbleButton>();
                    if (!opponentAbleButton.isOrderOutProcessing)
                    {
                        opponentAbleButton.isOrderOutProcessing = true;
                        AttachPlusBox(opponentParent, opponentPlusBox.GetComponentInParent<Transform>());
                        opponentAbleButton.isOrderOutProcessing = false;
                    }
                }
            }
        }
    }

    private void AttachPlusBox(Transform opponentParent, Transform opponentPlusBox)
    {
        //attach it first
        transform.SetParent(opponentParent, true);

        Vector3 possiblePosition = FindPlusBoxPossiblePosition(opponentParent, transform.localPosition);
        if (possiblePosition == Vector3.zero)
        {
            transform.SetParent(null);
            return;
        }

        isAttached=true;
        transform.localPosition = possiblePosition;
        transform.localRotation = opponentPlusBox.localRotation;
        transform.localScale = opponentPlusBox.localScale;
        if (HasButton())
        {
            GetComponentInParent<AbleButton>().AddPlusBoxOrderOut(this);
        }
    }

    private Vector3 FindPlusBoxPossiblePosition(Transform opponentParent, Vector3 thisLocalPosition)
    {
        List<Vector3> possiblePosition = new List<Vector3>();
        float ableDistance = 0.02f;
        for (int i = 0; i < opponentParent.childCount; i++)//add possiblePosition by distance
        {
            if (opponentParent.GetChild(i) != transform)
            {
                Vector3 curOpponetLocalPosition = opponentParent.GetChild(i).localPosition;
                Vector3 curOpponetSidePosition;

                curOpponetSidePosition = curOpponetLocalPosition + Vector3.left;
                if (Vector3.SqrMagnitude(curOpponetSidePosition - thisLocalPosition) < ableDistance)
                {
                    if (!possiblePosition.Contains(curOpponetSidePosition))
                        possiblePosition.Add(new Vector3(curOpponetSidePosition.x, curOpponetSidePosition.y, curOpponetSidePosition.z));
                }
                curOpponetSidePosition = curOpponetLocalPosition + Vector3.right;
                if (Vector3.SqrMagnitude(curOpponetSidePosition - thisLocalPosition) < ableDistance)
                {
                    if (!possiblePosition.Contains(curOpponetSidePosition))
                        possiblePosition.Add(new Vector3(curOpponetSidePosition.x, curOpponetSidePosition.y, curOpponetSidePosition.z));
                }
                curOpponetSidePosition = curOpponetLocalPosition + Vector3.up;
                if (Vector3.SqrMagnitude(curOpponetSidePosition - thisLocalPosition) < ableDistance)
                {
                    if (!possiblePosition.Contains(curOpponetSidePosition))
                        possiblePosition.Add(new Vector3(curOpponetSidePosition.x, curOpponetSidePosition.y, curOpponetSidePosition.z));
                }
                curOpponetSidePosition = curOpponetLocalPosition + Vector3.down;
                if (Vector3.SqrMagnitude(curOpponetSidePosition - thisLocalPosition) < ableDistance)
                {
                    if (!possiblePosition.Contains(curOpponetSidePosition))
                        possiblePosition.Add(new Vector3(curOpponetSidePosition.x, curOpponetSidePosition.y, curOpponetSidePosition.z));
                }
            }
        }

        for (int i = 0; i < opponentParent.childCount; i++)
        {
            if (possiblePosition.Count <= 0) return Vector3.zero;
            if(opponentParent.GetChild(i)!=transform)
            {
                if (possiblePosition.Contains(opponentParent.GetChild(i).localPosition))
                {
                    Debug.Log(transform.name+" has "+ opponentParent.GetChild(i).name);
                    possiblePosition.Remove(opponentParent.GetChild(i).localPosition);
                }
            }
        }
        if (possiblePosition.Count <= 0) return Vector3.zero;
        return possiblePosition[0];
    }

    public void DetachPlusBox()
    {
        isAttached = false;
        transform.SetParent(null, true);
    }

    public void DetroyPlusBox()
    {
        transform.SetParent(null, true);
        gameObject.SetActive(false);
    }
}

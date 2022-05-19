using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    private AbleButton ableButton;
    
    private Vector3 direction;
    private bool isMove = false;
    
    private Vector3 angle;
    private bool isRotate = false;

    private void Start()
    {
        ableButton = GetComponent<AbleButton>();
        direction = Vector3.zero;
        angle = Vector3.zero;
    }

    private void Update()
    {
        if (direction == Vector3.zero && angle == Vector3.zero)
        {
            //x
            if (Input.GetKey(KeyCode.A)){
                direction.x = -1;
                isMove = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                direction.x = 1;
                isMove = true;
            }

            //y
            else if (Input.GetKey(KeyCode.S))
            {
                direction.y = -1;
                isMove = true;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                direction.y = 1;
                isMove = true;
            }
            
            //rotate
            else if (Input.GetKey(KeyCode.Q))
            {
                angle.z = -1;
                isRotate = true;
            }
            else if (Input.GetKey(KeyCode.E))
        {
            angle.z = 1;
            isRotate = true;
        }

        }
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            ableButton.MoveOnPress(direction);
            isMove = false;
            direction = Vector3.zero;
        }
        else if (isRotate)
        {
            ableButton.RotateOnPress(angle);
            isRotate = false;
            angle = Vector3.zero;
        }
    }
}

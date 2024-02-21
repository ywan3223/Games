using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    public int playerNumber = 1;
    public bool isUIInput = false;

    Vector2 inputVector = Vector2.zero;
    TopDownCarController topDownCarController;

    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (isUIInput)
        {

        }
        else
        {
            inputVector = Vector2.zero;

            switch (playerNumber)
            {
                case 1:
                    inputVector.x = Input.GetAxis("Horizontal_P1");
                    inputVector.y = Input.GetAxis("Vertical_P1");
                    break;

                case 2:
                    inputVector.x = Input.GetAxis("Horizontal_P2");
                    inputVector.y = Input.GetAxis("Vertical_P2");
                    break;

            }
        }
        topDownCarController.SetInputVector(inputVector);
    }

    public void SetInput(Vector2 newInput)
    {
        inputVector = newInput;
    }
}

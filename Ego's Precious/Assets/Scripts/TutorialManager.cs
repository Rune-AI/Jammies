using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    private int buttonPressedCount = 0;

    public int buttonPressesNeeded = 30;

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Button Pressed");
        buttonPressedCount++;

        if (buttonPressedCount >= buttonPressesNeeded)
        {
            gameObject.SetActive(false);
        }
    }
}

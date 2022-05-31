using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerInputHandler : MonoBehaviour
{
    private GameManager gameMngr;

    public InputActionReference validateTrue;
    public InputActionReference validateFalse;
    public InputActionReference validate;
    public InputActionReference openMenuAction;
    public InputActionReference toggleDebugMenuAction;

    private void Awake()
    {
        gameMngr = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        //dialogBox = FindObjectOfType<GameManager>().GetComponent<DialogBox>();
        TaskInteractionInputHandler();

    }
    // KEYBOARD AND JOYSTICK INPUT
    private void TaskInteractionInputHandler()
    {
        openMenuAction.action.Enable();
        validateTrue.action.Enable();
        validateFalse.action.Enable();
        toggleDebugMenuAction.action.Enable();
        openMenuAction.action.performed += c => OnOpenMenu();
        //validate.action.performed += c => OnValidate();
        validateTrue.action.performed += c => OnValidateTrue();
        validateFalse.action.performed += c => OnValidateFalse();
        toggleDebugMenuAction.action.performed += c => OnToggleDebugMenu();
    }

    void OnOpenMenu()
    {
        Debug.Log("Open Menu");
        gameMngr.EndSession();
    }

    /*
    void OnValidate()
    {

        if (gameMngr.sessionStarted)
        {
            if (answer == 1)
            {
                gameMngr.OnPlayerInput(true);
                Debug.Log("PlayerInput: TRUE");
            }
            else if (answer == -1)
            {
                gameMngr.OnPlayerInput(false);
                Debug.Log("PlayerInput: FALSE");
            }
        }
        else
        {
            Debug.Log("Dialog box is opened, cannot validate");
        }

    }
    */

    void OnValidateTrue()
    {

        if (gameMngr.sessionStarted)
        {
            gameMngr.OnPlayerInput(true);
            Debug.Log("PlayerInput: TRUE");
        }
        else
        {
            Debug.Log("Dialog box is opened, cannot validate");
        }

    }

    void OnValidateFalse()
    {

        if (gameMngr.sessionStarted)
        {
            gameMngr.OnPlayerInput(false);
            Debug.Log("PlayerInput: FALSE");
        }
        else
        {
            Debug.Log("Dialog box is opened, cannot validate");
        }

    }

    void OnToggleDebugMenu()
    {
        if (gameMngr.debugUI.activeInHierarchy)
            gameMngr.debugUI.SetActive(false);
        else
            gameMngr.debugUI.SetActive(true);

    }
}

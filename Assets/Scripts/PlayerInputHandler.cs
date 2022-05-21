using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerInputHandler : MonoBehaviour
{
    private GameManager gameMngr;

    public InputActionReference validateAction;
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
        validateAction.action.Enable();
        toggleDebugMenuAction.action.Enable();
        openMenuAction.action.performed += c => OnOpenMenu();
        validateAction.action.performed += c => OnValidateAction();
        toggleDebugMenuAction.action.performed += c => OnToggleDebugMenu();
    }

    void OnOpenMenu()
    {
        Debug.Log("Open Menu");
        gameMngr.EndSession();
    }

    void OnValidateAction()
    {

        if (gameMngr.sessionStarted)
        {
            //gameMngr.OnPlayerInput(answer);
            Debug.Log("PlayerInput");
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

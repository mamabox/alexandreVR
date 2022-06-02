using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogBox : MonoBehaviour
{

    //private GameManager gameManager;
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI buttonText;
    public Button dialogBoxBtn;
    private string dialogBoxMode;
    //private charController charController;

    private GameManager gameMngr;


    private void Awake()
    {
       
        gameMngr = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        //charController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<charController>();
        dialogBoxMode = "session";

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void OpenDialogBox(string text, string mode)
    {
        this.gameObject.SetActive(true);
        gameMngr.ShowControllers(true);
        //charController.freezeMovement = true;
        gameMngr.freezePlayer = true;
        //Debug.Log("Open a dialog box with text: " + text);
        instructions.gameObject.SetActive(true);
        instructions.text = text.Replace("|", System.Environment.NewLine);
        dialogBoxMode = mode;

        if (dialogBoxMode == "session")
        {
            buttonText.text = "Commencer";
        }
        else if (dialogBoxMode == "trial" || dialogBoxMode == "endDemo")
        {
            buttonText.text = "Commencer";
        }
        else if (dialogBoxMode == "end")
        {
            buttonText.text = "Recommencer";
        }
        else if (dialogBoxMode == "pause")
        {
            buttonText.text = "Continuer";
        }
    }


    public void CloseDialogBox()
    {
        gameMngr.ShowControllers(false);
        if (dialogBoxMode == "session")
        {
            gameMngr.StartSession();
        }
        else if (dialogBoxMode == "trial" || dialogBoxMode == "pause" || dialogBoxMode == "endDemo")
        {
            gameMngr.StartTrial();
        }
        else if (dialogBoxMode == "end")
        {
            gameMngr.StartSession();
        }
        else if (dialogBoxMode != "none")
        {
            Debug.Log("Mode not reconized");
        }


    }
}

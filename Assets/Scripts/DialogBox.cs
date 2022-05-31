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
    public TextMeshProUGUI instructionsImg;
    public TextMeshProUGUI instructions;
    public GameObject instructionsWithImg;
    public Button dialogBoxBtn;
    public RawImage image;
    private string imagePath;
    private Texture2D myTexture;
    //private string fileName;
    private byte[] bytes;
    private string dialogBoxMode;
    //private charController charController;

    private GameManager gameMngr;


    private void Awake()
    {
        imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Media/Images/");
        gameMngr = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        //charController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<charController>();
        dialogBoxMode = "not set";
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
   
        //charController.freezeMovement = true;

        //Debug.Log("Open a dialog box with text: " + text);
        instructions.gameObject.SetActive(true);
        instructions.text = text.Replace("|", System.Environment.NewLine);
        dialogBoxMode = mode;

    }


    public void CloseDialogBox()
    {

     if (dialogBoxMode == "trial")
        {
            gameMngr.StartTrial();
        }
        else if (dialogBoxMode == "end")
        {
            gameMngr.EndSession();
        }
        else if (dialogBoxMode != "none")
        {
            Debug.Log("Mode not reconized");
        }


    }
}

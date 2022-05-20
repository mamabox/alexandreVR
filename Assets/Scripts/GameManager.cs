using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    private StimuliManager stimuliMngr;

    //UI Elements
    public GameObject menuUI;
    public GameObject debugUI;
    public GameObject dialogBox;

    public TextMeshProUGUI taskNbTxt;
    public TextMeshProUGUI totalTimeTxt;
    public TextMeshProUGUI trialTimeTxt;
    public TextMeshProUGUI lastTrialTxt;

    private int triaNb;
    private int totalTrialNb;

    //Time
    private float startTime;
    public float taskStartTime;
    public float endTime;
    private TimeSpan totalTime;
    public TimeSpan trialTime;

    private bool playerChoice;

    public bool sessionStarted;

    //Save Data
    string savePath;

    private void Awake()
    {
       

        //totalTrialNb =
        triaNb = 0;

        //OpenMenu();
       SetSavePath();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
        UpdateUI();
    }

    void StartSession()
    {
        Debug.Log("Startsession");
        sessionStarted = true;
        startTime = Time.time;
        triaNb = 0;
    }

    private void StartTrial()
    {

    }

    private void EndTrial()
    {


        if (triaNb < totalTrialNb)
        {
            totalTrialNb++;
            StartTrial();
        }
        else
            EndSession();

    }

    void EndSession()
    {
        Debug.Log("EndSession");
        sessionStarted = false;
    }

    private void OpenMenu()
    {
        dialogBox.SetActive(false);
        menuUI.SetActive(true);
    }


    private void SetSavePath()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Path.Combine(Application.persistentDataPath + "/Export/");
        }
        else
        {
            savePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Data/Export/"); //For macOS, Linux
        }
        if (!System.IO.Directory.Exists(savePath))    //if save directory does not exist, create it
        {
            Debug.Log("Create " + savePath + " directory");
            System.IO.Directory.CreateDirectory(savePath);
        }
        else
            Debug.Log(savePath + " exists");
    }

    private void UpdateClock()
    {
        totalTime = TimeSpan.FromSeconds(Time.time - startTime);
        if (sessionStarted)  //if session is ongoing
        {
            trialTime = TimeSpan.FromSeconds(Time.time - taskStartTime);
            //Debug.Log("TimeSpan" + TimeSpan.FromSeconds(0));
        }
        else
            trialTime = TimeSpan.FromSeconds(0);
    }

    private void UpdateUI()
    {
        totalTimeTxt.text = "Total: " + totalTime.ToString(@"mm\:ss");
        trialTimeTxt.text = "Task: " + trialTime.ToString(@"mm\:ss");
        taskNbTxt.text = "Trial " + triaNb + " / " + totalTrialNb;

    }

    private void OnPlayerInput(bool input)
    {
        playerChoice = input;
        SaveData();
        EndTrial();
        
    }

    private void SaveData()
    {

    }


}

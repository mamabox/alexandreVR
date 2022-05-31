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

    private int trialNb;
    private int totalTrialNb;

    //Time
    private float startTime;
    public float taskStartTime;
    public float endTime;
    private TimeSpan totalTime;
    public TimeSpan trialTime;

    private bool playerChoice;
    public bool openUI;
    public bool sessionStarted;

    //Save Data
    string savePath;
    public string importPath;
    public TrialsData trialsData;
    public TextAsset jsonFile;

    private void Awake()
    {
       stimuliMngr = GameObject.Find("Stimuli Manager").GetComponent<StimuliManager>();

        //totalTrialNb =
        trialNb = 0;

        //OpenMenu();
       SetSavePath();
    }

    void Start()
    {
        StartSession();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
        //UpdateUI();
    }

    public void StartSession()
    {
        Debug.Log("Startsession");
        sessionStarted = true;
        startTime = Time.time;
        trialNb = 1;
        totalTrialNb = trialsData.trials.Count;
        StartTrial();
    }

    private void StartTrial()
    {
        string _hint = trialsData.trials[trialNb-1].hintID;
        //float _duration = trialsData.trials[trialNb - 1].hintDuration;
        float _duration = 3;
        List<string> _stimuli = trialsData.trials[trialNb-1].stimuli;

        Debug.Log("TRIAL: " + trialNb + " / " + totalTrialNb + " SHOW: " + _hint + " (hint)" + " + " + string.Join(",", _stimuli));
        

        stimuliMngr.HideAll();

        //stimuliMngr.ShowHintByName(_hint,_duration);

        StartCoroutine(stimuliMngr.ShowHint(_hint,_duration,_stimuli));

        for (int x = 0; x < _stimuli.Count; x++)
        {
            //stimuliMngr.ShowStimulusByName(_stimuli[x]);
        }

    }


    private void EndTrial()
    {


        if (trialNb < totalTrialNb)
        {
            trialNb++;
            StartTrial();
        }
        else
            EndSession();

    }

    public void EndSession()
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
        taskNbTxt.text = "Trial " + trialNb + " / " + totalTrialNb;

    }

    public void OnPlayerInput(bool input)
    {
        playerChoice = input;
        SaveData();
        EndTrial();
        
    }

    private void SaveData()
    {

    }


}

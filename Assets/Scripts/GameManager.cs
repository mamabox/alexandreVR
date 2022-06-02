using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private StimuliManager stimuliMngr;

    //UI Elements
    public GameObject menuUI;
    public GameObject debugUI;
    public GameObject dialogBox;

    public TextMeshProUGUI trialNbTxt;
    public TextMeshProUGUI totalTimeTxt;
    public TextMeshProUGUI trialTimeTxt;
    public TextMeshProUGUI lastTrialTxt;

    public List<GameObject> controllers;

    private int trialNb;
    private int totalTrialNb;
    private int demoTrialNb;
    private int trialNbPause;
    private bool demoTrials;

    //UI
    private string demoUIText;

    //Time
    private float startTime;
    public float trialStartTime;
    public float endTime;
    private TimeSpan totalTime;
    public TimeSpan trialTime;
    public TimeSpan lastTrialTime;

    private bool playerChoice;
    public bool openUI;
    public bool sessionStarted;
    public bool freezePlayer;

    //Save Data
    string savePath;
    public string importPath;
    public TrialsData trialsData;
    public TextAsset jsonFile;
    private int condition;
    private bool isAnswerCorrect;
    private StreamWriter sw;
    private string fileName;
    private char fileNameDelimiter = '-';
    private char delimiter = ',';
    string participantID = "0";
    string dateTime;
    string savingStatusText;

    private void Awake()
    {
        stimuliMngr = GameObject.Find("Stimuli Manager").GetComponent<StimuliManager>();

        //totalTrialNb =
        trialNb = 0;
        freezePlayer = true;
        savingStatusText = "[Not saving]";

        //OpenMenu();
        SetSavePath();
    }

    void Start()
    {
        //StartSession();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
        UpdateUI();
    }

    public void StartSession()
    {
        Debug.Log("Startsession");
        sessionStarted = true;
        startTime = Time.time;
        trialNb = 1;
        trialNbPause = trialsData.trialNbPause;
        totalTrialNb = trialsData.trials.Count;
        demoTrialNb = CountDemoTrials();
        freezePlayer = true;
        StartSavingData();
        StartTrial();
    }

    public void StartTrial()
    {
        string _hint = trialsData.trials[trialNb - 1].hintID;
        float _duration = trialsData.trials[trialNb - 1].hintDuration / 1000;
        condition = trialsData.trials[trialNb - 1].condition;
        //float _duration = 3;
        List<string> _stimuli = trialsData.trials[trialNb - 1].stimuli;
        IsDemoTrial();

        Debug.Log("TRIAL: " + trialNb + " / " + totalTrialNb + demoUIText + " SHOW: " + _hint + " (hint)" + " + " + string.Join(",", _stimuli));


        //stimuliMngr.HideAll();

        //stimuliMngr.ShowHintByName(_hint,_duration);

        StartCoroutine(stimuliMngr.TrialDisplay(_hint, _duration, _stimuli));


    }

    private void IsDemoTrial()
    {
        if (trialNb <= demoTrialNb) {
            demoTrials = true;
            demoUIText = " (DEMO)";
            savingStatusText = "[Not saving]";
        }
        else
        {
            demoTrials = false;
            demoUIText = "";
            savingStatusText = "[Saving]";
        }

    }

    private int CountDemoTrials()
    {
        int _demoNb = 0;
        foreach (Trial trial in trialsData.trials) {
            if (trial.condition == 0)
                _demoNb++;
                }
        Debug.Log("Nb of demo trials = " + _demoNb);
        return _demoNb;

    }

    public void ShowControllers(bool status)
    {
        foreach (GameObject controller in controllers)
        {
            if (status)
                controller.SetActive(true);
            else
                controller.SetActive(false);
        }
    }


    private void EndTrial()
    {
        stimuliMngr.HideAll();
        //IF there are trials left
        if (trialNb < totalTrialNb)
        {
                if (trialNb == demoTrialNb) // IF this is the last demo trial
											{
                trialNb++;
                dialogBox.GetComponent<DialogBox>().OpenDialogBox(trialsData.instructions.endDemo, "endDemo");
            }

                else if ((trialNb -demoTrialNb) % trialNbPause == 0)    // if this a pause moment
                {
                    trialNb++;
                    dialogBox.GetComponent<DialogBox>().OpenDialogBox(trialsData.instructions.pause, "pause");
                    //dialogBox.GetComponent<DialogBox>().OpenDialogBox("string, "pause");
                }
                else // move on the the next trial
                {
                    trialNb++;
                    StartTrial();
                }


        }
        else
            EndSession();

    }

    public void EndSession()
    {
        Debug.Log("EndSession");
        sessionStarted = false;
        freezePlayer = true;
        dialogBox.GetComponent<DialogBox>().OpenDialogBox(trialsData.instructions.end, "session");
        StopSavingData();
        stimuliMngr.HideAll();
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
        if (sessionStarted && !freezePlayer)  //if session is ongoing
        {
            trialTime = TimeSpan.FromSeconds(Time.time - trialStartTime);
            //Debug.Log("TimeSpan" + TimeSpan.FromSeconds(0));
        }
        else
            trialTime = TimeSpan.FromSeconds(0);

    }

    private void UpdateUI()
    {
        totalTimeTxt.text = "Total: " + totalTime.ToString(@"mm\:ss");
        trialTimeTxt.text = "Trial: " + trialTime.ToString(@"ss\:fff");
        trialNbTxt.text = "COND " + condition + " - Trial " + trialNb + " / " + totalTrialNb + demoUIText; ;
        if (sessionStarted && trialNb > 1)  //if session is ongoing
        {
            if (isAnswerCorrect)
                lastTrialTxt.text = "Trial " + (trialNb - 1) + delimiter + ": correct" + " - " + lastTrialTime.ToString(@"ss\:fff") + " (" + playerChoice + ")" + savingStatusText;
            else

                lastTrialTxt.text = "Trial " + (trialNb - 1) + delimiter + ": incorrect" + " - " + lastTrialTime.ToString(@"ss\:fff") + " (" + playerChoice + ")" + savingStatusText;
        }
        else
            lastTrialTxt.text = "Last trial";
    }

    public void OnPlayerInput(bool input)
    {

        playerChoice = input;
        lastTrialTime = trialTime;
        CheckIfCorrect();
        if (!demoTrials)
            SaveData();
        EndTrial();

    }

    private void CheckIfCorrect()
    {
        if ((condition == 1 || condition == 2) && playerChoice)
        {
            isAnswerCorrect = true;
        }
        else if (condition == 3 && !playerChoice)
            isAnswerCorrect = true;
        else
        {
            isAnswerCorrect = false;
        }

    }

    private void StartSavingData()
    {
        SetFileName();
        sw = File.AppendText(savePath + fileName);
        //sw.WriteLine("this is a test");
        sw.WriteLine(HeadersConstructor()); //Add Headers to the file
    }

    private void SetFileName()
    {
        dateTime = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        fileName = dateTime + fileNameDelimiter + participantID + ".csv";
    }

    private string HeadersConstructor()
    {
        string sessionHeader = "dateTime" + delimiter + "participantID";
        string trialHeader = "trialNb" + delimiter + "condition" + delimiter + "hintID" + delimiter + "hintDelay" + delimiter + "stimuli";
        string responseHeader = "correctness" + delimiter + "reactionTime" + delimiter + "answer";

        return sessionHeader + delimiter + trialHeader + delimiter + responseHeader;
    }

    private void SaveData()
    {
        int adjustedTrialNb = trialNb - demoTrialNb;
        Trial _trial = trialsData.trials[trialNb];
        string correctness;
        

        Debug.Log("SaveData");
        string sessionData = dateTime + delimiter + participantID;
        //string trialData = adjustedTrialNb.ToString() + _trial.condition + delimiter + _trial.hintID + delimiter + _trial.hintDuration + delimiter + String.Join("_, _trial.stimuli);
        string trialData = adjustedTrialNb.ToString() + delimiter + _trial.condition + delimiter + _trial.hintID + delimiter + _trial.hintDuration + delimiter + String.Join("_", _trial.stimuli);
        if (isAnswerCorrect)
            correctness = "correct";
        else
            correctness = "incorrect";
        string responseData = correctness + delimiter + lastTrialTime.ToString(@"ss\:fff") + delimiter + playerChoice;

        sw.WriteLine(sessionData + delimiter + trialData + delimiter + responseData);
    }

    public void StopSavingData()
    {
        sw.Close();

    }

}

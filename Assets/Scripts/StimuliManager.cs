using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StimuliManager : MonoBehaviour
{
    public List<GameObject> stimuliPlanePrefabs;
    public List<GameObject> allStimuli;
    public List<GameObject> hintPrefabs;
    public List<GameObject> allHints;

    private GameManager gameMngr;

    private int stimuliCount;
    private int hintsCount;
    private char delimiter = '_';

    private void Awake()
    {
        gameMngr = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        ConfigureStimuli();
        HideAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        //ShowStimuliByName("1_I");
        //ShowStimuliByName("1_U_triangleR");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ConfigureStimuli()
    {
        for (int index = 0; index < stimuliPlanePrefabs.Count; index++) {     // for each plane
            foreach (Transform stimuli in stimuliPlanePrefabs[index].transform) // for each stimuli
            {
                HintPrefab _hint = stimuli.GetComponent<StimuliPrefab>().hint.GetComponent<HintPrefab>();
                _hint.position = stimuli.GetComponent<StimuliPrefab>().position;
                _hint.plane = index + 1;
                _hint.ID = _hint.plane.ToString() + delimiter + _hint.position.ToUpper();
                _hint.name = "hint_" + _hint.ID;
                allHints.Add(_hint.gameObject);

                foreach (GameObject stimulus in stimuli.GetComponent<StimuliPrefab>().stimuli) // for each stimulus
                {
                    StimulusPrefab _stimulus = stimulus.GetComponent<StimulusPrefab>();

                    // Configure stimuli properties and name
                    _stimulus.position = stimuli.GetComponent<StimuliPrefab>().position; //Sets the position
                    _stimulus.plane = index + 1;
                    _stimulus.ID = _stimulus.plane.ToString() + delimiter + _stimulus.position.ToUpper() + delimiter + _stimulus.shape + _stimulus.color.ToUpper();
                    _stimulus.name = _stimulus.ID;

                    // Add stimuli to allStimuli list
                    allStimuli.Add(_stimulus.gameObject);
                    //_stimulus.gameObject.SetActive(false);
                    
                }
            }
        }
        stimuliCount = allStimuli.Count;
        hintsCount = allHints.Count;
    }

    private void ConfigureHints()
    {
        for (int index = 0; index < hintPrefabs.Count; index++) // for each plane
        {
            foreach (Transform hint in hintPrefabs[index].transform) // for each position
            {
                HintPrefab _hint = hint.GetComponent<HintPrefab>();

                // Configure stimuli properties and name
                _hint.ID = index+1.ToString() + delimiter + _hint.position.ToUpper();
                _hint.name = "hint_"+_hint.ID;

                // Add stimuli to allStimuli list
                allHints.Add(_hint.gameObject);
                //_stimulus.gameObject.SetActive(false);

            }
        }
    
    hintsCount = allHints.Count;
    }

public void HideAll()
    {
        foreach (GameObject stimulus in allStimuli)
        {
            stimulus.gameObject.SetActive(false);
        }

        foreach (GameObject hint in allHints)
        {
            hint.gameObject.SetActive(false);
        }
    }

    private int ReturnStimulusIndex(string name)
    {
        for (int index = 0; index < stimuliCount; index ++)
        {
            if (allStimuli[index].GetComponent<StimulusPrefab>().ID == name)
            {
                //Debug.Log(name + " has index #: " + index);
                return index;
            }
        }

        return 999;
    }

    private int ReturnHintIndex(string name)
    {
        for (int index = 0; index < hintsCount; index++)
        {
            if (allHints[index].GetComponent<HintPrefab>().ID == name)
            {
                //Debug.Log(name + " has index #: " + index);
                return index;
            }
        }

        return 999;
    }

    public void ShowStimulusByName(string name)
    {
        int _index = ReturnStimulusIndex(name);

        if (_index == 999)
            Debug.Log("ERROR: No stimuli with name '" + name + "'");
        else
            allStimuli[_index].SetActive(true);
    }

    /*
    public void ShowHintByName(string name, float duration)
    {
        int _index = ReturnHintIndex(name);
        float timePassed = 0;

        if (_index == 999)
            Debug.Log("ERROR: No hint with name '" + name + "'");
        else
        {
            gameMngr.freezePlayer = true;
            allHints[_index].SetActive(true);
            Debug.Log("SHOW hint: " + name + " with index #: " + _index);
            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;
                Debug.Log("timePassed = " + timePassed);
            }
            
            Debug.Log("HIDE hint: " + name + " with index #: " + _index);
            allHints[_index].SetActive(false);
            gameMngr.freezePlayer = false;
            gameMngr.trialStartTime = Time.time;
        }
    }
    */


        public IEnumerator TrialDisplay(string name, float duration, List<string> stimuli)
        {
            int _index = ReturnHintIndex(name);

            if (_index == 999)
                Debug.Log("ERROR: No hint with name '" + name + "'");
        else
            {
            gameMngr.freezePlayer = true;
            allHints[_index].SetActive(true);
                //Debug.Log("SHOW hint: " + name + " with index #: " + _index);

                yield return new WaitForSeconds(duration);

                //Debug.Log("HIDE hint: " + name + " with index #: " + _index);
                allHints[_index].SetActive(false);
            gameMngr.freezePlayer = false;
            gameMngr.trialStartTime = Time.time;

            for (int x = 0; x < stimuli.Count; x++)
            {
                ShowStimulusByName(stimuli[x]);
            }
        }

        }
    }


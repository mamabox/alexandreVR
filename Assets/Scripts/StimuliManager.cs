using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StimuliManager : MonoBehaviour
{
    public List<GameObject> planePrefabs;
    public List<GameObject> allStimuli;
    public List<GameObject> hintPrefabs;
    public List<GameObject> allHints;

    private int stimuliCount;
    private int hintsCount;
    private char delimiter = '_';

    private void Awake()
    {
        ConfigureStimuli();
        HideAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowStimuliByName("1_I");
        ShowStimuliByName("1_U_triangleR");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ConfigureStimuli()
    {
        for (int index = 0; index < planePrefabs.Count; index++) {     // for each plane
            foreach (Transform stimuli in planePrefabs[index].transform) // for each stimuli
            {
                foreach (Transform stimulus in stimuli.transform) // for each stimulus
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
                _hint.name = _hint.ID;

                // Add stimuli to allStimuli list
                allHints.Add(_hint.gameObject);
                //_stimulus.gameObject.SetActive(false);

            }
        }
    
    hintsCount = allHints.Count;
    }

private void HideAll()
    {
        foreach (GameObject stimulus in allStimuli)
        {
            stimulus.gameObject.SetActive(false);
        }
    }

    private int ReturnStimulusIndex(string name)
    {
        for (int index = 0; index < stimuliCount; index ++)
        {
            if (allStimuli[index].GetComponent<StimulusPrefab>().name == name)
            {
                Debug.Log(name + " has index #: " + index);
                return index;
            }
        }

        return 999;
    }

    private void ShowStimuliByName(string name)
    {
        int _index = ReturnStimulusIndex(name);

        if (_index == 999)
            Debug.Log("ERROR: No stimuli with name '" + name + "'");
        else
            allStimuli[_index].SetActive(true);
    }
}

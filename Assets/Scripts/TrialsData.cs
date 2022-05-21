using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrialsData
{
    public List<Trial> trials;
}

[System.Serializable]
public class Trial
{
    public int trialNb;
    public int condition;
    public string hintID;
    public float hintDuration;
    public List<string> stimuli;
}

    [System.Serializable]
    public class Location
    {
    public string position;
    public int plane;
    }

    [System.Serializable]
    public class Stimuli
    {
        public List<Stimulus> stimulus;
    }

    
    [System.Serializable]
    public class Stimulus
    {
        public string shape;
        public string color;
        public Location location;
    }



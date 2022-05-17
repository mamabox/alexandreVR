using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialsData
{
    [System.Serializable]
    public class Trial
    {
        int trialNb;
        int condition;
        Location hintlocation;
        float hintDuration;
        List<Stimuli> stimuli;
    }

    [System.Serializable]
    public class Location
    {
        string position;
        int plane;
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


}

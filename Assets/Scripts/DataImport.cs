using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class DataImport : MonoBehaviour
{
    private string dataFile;
    private string dataFileAndroid;
    
    private GameManager gameMngr;
    
    

    void Awake()
    {
        //Initialisation

        gameMngr = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        gameMngr.importPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Data/Import/");
        dataFile = gameMngr.importPath + "SessionData.json";
        dataFileAndroid = Path.Combine(Application.streamingAssetsPath + "/" + "SessionData.json");

        ImportTaskData();

    }

    public void ImportTaskData()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Start Coroutine");
            
            StartCoroutine("AndroidDataImport");
            
        }
        else
        {

            Debug.Log("Read from jsonFile");
            ImportTaskData(dataFile);
            //gameMngr.trials = JsonUtility.FromJson<TrialsData>(gameMngr.jsonFile.text);
            

        }
        //gameManager.UpdateUIAndroid();
    }

    IEnumerator AndroidDataImport()
    {
        Debug.Log("IEnumertor");
        string sJson;

        UnityWebRequest www2 = UnityWebRequest.Get(dataFileAndroid);
        yield return www2.SendWebRequest();
        if (www2.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error reading data from  " + dataFileAndroid);
            Debug.Log(www2.error);
        }
        else
        {
            sJson = www2.downloadHandler.text;
            //Debug.Log(sJson);
            gameMngr.trialsData = JsonUtility.FromJson<TrialsData>(sJson);
            Debug.Log("Data imported");
            gameMngr.dialogBox.GetComponent<DialogBox>().OpenDialogBox(gameMngr.trialsData.instructions.start, "session"); // TEMP
        }
    }

    public void ImportTaskData(string dataFile)
    {
        if (File.Exists(dataFile))
        {
            Debug.Log(dataFile + " exists");
            string fileContents = File.ReadAllText(dataFile);                   // Read the entire file and save its contents.
            gameMngr.trialsData = JsonUtility.FromJson<TrialsData>(fileContents);                // Deserialize the JSON data into a pattern matching the GameData class.


        }
        else
        {
            Debug.Log(dataFile + " does not exist");
        }

        //scenarioManager.scenario7Data = scenarioManager.ImportScenarioStdDataJson(7);
    }

}

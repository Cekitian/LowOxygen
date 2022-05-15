using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public Data DATA = new Data();

    public class Data
    {
        public int currentCheckPoint;
        public int timePassed;
        public int timePassedCurrentRun;
        public int deaths;
        public int jumps;
        public int canistersCollected;
        public bool[] hasHat;
        public bool wonGame;
        public int recordTime;
        public bool gotAllHats;
        
        public Data()
        {
            currentCheckPoint = 0;
            timePassed = 0;
            timePassedCurrentRun = 0;
            deaths = 0;
            jumps = 0;
            canistersCollected = 0;
            hasHat = new bool[5];
            wonGame = false;
            recordTime = -1;
            gotAllHats = false;
        }
    }
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;
        
        if(FileManager.GetDataFromFile() == null)
        {
            SaveData();
            DATA = JsonUtility.FromJson<Data>(FileManager.GetDataFromFile());
        }
        else
        {
            DATA = JsonUtility.FromJson<Data>(FileManager.GetDataFromFile());
        }

    }
    /// <summary>
    /// Saves the data on the machine locally
    /// </summary>
    public void SaveData()
    {
        string textToWrite = JsonUtility.ToJson(DATA);
        FileManager.SaveDataToFile(textToWrite);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}

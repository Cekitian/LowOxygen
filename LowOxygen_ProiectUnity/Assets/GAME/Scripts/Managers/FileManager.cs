using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileManager
{
    public static string GetDataFromFile()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.txt"))
            return File.ReadAllText(Application.persistentDataPath + "/gameData.txt");
        else
            return null;
    }
    public static void SaveDataToFile(string data)
    {
        File.WriteAllText(Application.persistentDataPath + "/gameData.txt", data);
    }  
}

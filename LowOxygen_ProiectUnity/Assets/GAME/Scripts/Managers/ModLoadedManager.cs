using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using TMPro;

public class ModLoadedManager : MonoBehaviour
{
    public static ModLoadedManager Instance;
    private string modFolderDataPath;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    public void SetDataPath(string dataPath)
    {
        modFolderDataPath = dataPath;
    }
    public string GetDataPath()
    {
        return modFolderDataPath;
    }
    public List<string> GetRoomGRMap()
    {
        List<string> roomPaths = new List<string>();

        //search for rooms
        //rooms naming convention for ground : room_GR1
        //rooms naming convention for background : room_BG1

        int currentSearchedRoom = 1;

        while(File.Exists(modFolderDataPath +"/room_GR" +  currentSearchedRoom.ToString()))
        {
            roomPaths.Add(modFolderDataPath + "/room_GR" + currentSearchedRoom.ToString());
            currentSearchedRoom++;
        }
        return roomPaths;
    }  
    public List<string> GetRoomBGMap()
    {
        List<string> roomPaths = new List<string>();

        //search for rooms
        //rooms naming convention for ground : room_GR1
        //rooms naming convention for background : room_BG1

        int currentSearchedRoom = 1;

        while (File.Exists(modFolderDataPath + "/room_BG" + currentSearchedRoom.ToString()))
        {
            roomPaths.Add(modFolderDataPath + "/room_BG" + currentSearchedRoom.ToString());
            currentSearchedRoom++;
        }
        return roomPaths;
    }
    public List<string> GetRoomOBJMap()
    {
        List<string> roomPaths = new List<string>();

        //search for rooms
        //rooms naming convention for ground : room_GR1
        //rooms naming convention for background : room_BG1

        int currentSearchedRoom = 1;

        while (File.Exists(modFolderDataPath + "/room_OBJ" + currentSearchedRoom.ToString()))
        {
            roomPaths.Add(modFolderDataPath + "/room_OBJ" + currentSearchedRoom.ToString());
            currentSearchedRoom++;
        }
        return roomPaths;
    }
}

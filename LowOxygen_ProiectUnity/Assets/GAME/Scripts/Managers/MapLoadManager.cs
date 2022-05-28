using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.IO;

public class MapLoadManager : MonoBehaviour
{
    public static MapLoadManager Instance;

    public List<GameObject> spawnedGameObjects = new List<GameObject>();
    public GameObject theRoom;
    [SerializeField]
    private GameObject emptyRoomPrefab;
    [SerializeField]
    private List<TileObject> tiles;
    [SerializeField]
    private List<RoomObject> objects;
   [SerializeField] private GameObject finishLine;

    private void Awake()
    {
        Instance = this;

        List<string> roomsGR = ModLoadedManager.Instance.GetRoomGRMap();
        List<string> roomsBG = ModLoadedManager.Instance.GetRoomBGMap();
        List<string> roomsOBJ = ModLoadedManager.Instance.GetRoomOBJMap();

        for(int i = 0; i < roomsGR.Count; i++)
        {
            GameObject room = Instantiate(emptyRoomPrefab);
            theRoom = room;
            GridManager grid = room.GetComponentInChildren<GridManager>();
            LoadTileMap(roomsGR[i], grid.groundTileMap);
            LoadTileMap(roomsBG[i], grid.backgroundTileMap);
            LoadRoomData(roomsOBJ[i], room);
            
            if(EditorManager.Instance != null)
            EditorManager.Instance.AddRoomToList(room, grid.groundTileMap, grid.backgroundTileMap, i+1);
        }
    }
    public class RoomTileData
    {
        public int roomId;
        public List<Vector3Int> tilePos = new List<Vector3Int>();
        public List<string> tileId = new List<string>();
    }
    public class RoomObjectData
    {
        [System.Serializable]
        public class SwitchObject
        {
            public int instanceId;
            public string objectId;
            public Vector3 pos;
            public bool initState;
        }
        public List<SwitchObject> switches = new List<SwitchObject>();
        [System.Serializable]
        public class PlantObject
        {
            public int instanceId;
            public string objectId;
            public Vector3 pos;
        }
        public List<PlantObject> plants = new List<PlantObject>();
        [System.Serializable]
        public class PressableButton
        {
            public int instanceId;
            public string objectId;
            public Vector3 pos;
            public List<int> connectedSwitches = new List<int>();
        }
        public List<PressableButton> buttons = new List<PressableButton>();
        [System.Serializable]
        public class Plate
        {
            public int instanceId;
            public string objectId;
            public Vector3 pos;
            public List<int> connectedSwitches = new List<int>();
        }
        public List<Plate> plates = new List<Plate>();

        public List<GameObject> finishLines = new List<GameObject>();
        public List<Vector3> finishPos = new List<Vector3>();
    }

    public void SaveTileMap(string savePath, Tilemap map)
    {
        RoomTileData data = new RoomTileData();

        for(int i = map.cellBounds.xMin ; i <= map.cellBounds.xMax;i++)
        {
            for (int j = map.cellBounds.yMin; j <= map.cellBounds.yMax; j++)
            {
                TileBase tile = map.GetTile(new Vector3Int(i, j, 0));

                TileObject findTile = tiles.Find(t => t.theTile == tile);

                if(findTile != null)
                {
                    data.tilePos.Add(new Vector3Int(i, j, 0));
                    data.tileId.Add(findTile.id);
                }
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath , json);
    }
    public void SaveRoomData(string savePath, List<GameObject> roomObjects)
    {
        Debug.Log("saving object data");
        RoomObjectData objectData = new RoomObjectData();

        foreach(GameObject x in roomObjects)
        {
            if(x.TryGetComponent(out Switch y))
            {
                RoomObjectData.SwitchObject newSwitch = new RoomObjectData.SwitchObject();
                newSwitch.pos = y.gameObject.transform.localPosition;
                newSwitch.initState = y.GetState();

                RoomObject findObject = objects.Find(o => o.objectId == y.GetComponent<EditorObject>().objectId);
                newSwitch.objectId = findObject.objectId;
                newSwitch.instanceId = y.GetComponent<EditorObject>().instanceId;
                if (findObject != null)
                {
                    objectData.switches.Add(newSwitch);
                }
            } else if(x.TryGetComponent(out Plant z))
            {
                Debug.Log("found plant");
                RoomObjectData.PlantObject newPlant = new RoomObjectData.PlantObject();
                newPlant.pos = z.gameObject.transform.localPosition;

                RoomObject findObject = objects.Find(o => o.objectId == z.GetComponent<EditorObject>().objectId);
                newPlant.objectId = findObject.objectId;
                newPlant.instanceId = z.GetComponent<EditorObject>().instanceId;
                Debug.Log(findObject);
                if (findObject != null)
                {
                    Debug.Log("matches type");
                    objectData.plants.Add(newPlant);
                }
            }
            else if(x.TryGetComponent(out ObjectButton v))
            {
                RoomObjectData.PressableButton newButton = new RoomObjectData.PressableButton();
                newButton.pos = v.gameObject.transform.localPosition;

                RoomObject findObject = objects.Find(o => o.objectId == v.GetComponent<EditorObject>().objectId);
                newButton.objectId = findObject.objectId;
                newButton.instanceId = v.GetComponent<EditorObject>().instanceId;
                newButton.connectedSwitches = v.GetComponent<EditorObject>().relatedIds;
                Debug.Log(findObject);
                if (findObject != null)
                {
                    objectData.buttons.Add(newButton);
                }
            }
            else if (x.TryGetComponent(out PressureButton w))
            {
                RoomObjectData.Plate newPlate = new RoomObjectData.Plate();
                newPlate.pos = w.gameObject.transform.localPosition;

                RoomObject findObject = objects.Find(o => o.objectId == w.GetComponent<EditorObject>().objectId);
                newPlate.objectId = findObject.objectId;
                newPlate.instanceId = w.GetComponent<EditorObject>().instanceId;
                newPlate.connectedSwitches = w.GetComponent<EditorObject>().relatedIds;
                Debug.Log(findObject);
                if (findObject != null)
                {
                    objectData.plates.Add(newPlate);
                }
            }
            else
            {
                objectData.finishLines.Add(x);
                objectData.finishPos.Add(x.transform.position);
            }
        }
        Debug.Log(objectData.plants);
        Debug.Log(objectData.switches);

        string json = JsonUtility.ToJson(objectData, true);
        File.WriteAllText(savePath, json);
    }
    public void LoadTileMap(string loadPath, Tilemap map)
    {
        string json = File.ReadAllText(loadPath);
        RoomTileData data = JsonUtility.FromJson<RoomTileData>(json);

        map.ClearAllTiles();

        for(int i = 0; i < data.tilePos.Count; i++)
        {           
            TileObject tempTile = tiles.Find(t => data.tileId[i] == t.id);
            Debug.Log(tempTile);
            map.SetTile(data.tilePos[i], tempTile.theTile);
        }
    }
    public void LoadRoomData(string loadPath, GameObject roomParent)
    {
        string json = File.ReadAllText(loadPath);
        RoomObjectData objectData = JsonUtility.FromJson<RoomObjectData>(json);

        for(int i = 0; i < objectData.switches.Count; i++)
        {
            RoomObject newSwitch = objects.Find(sw => sw.objectId == objectData.switches[i].objectId);

            GameObject theObj = Instantiate(newSwitch.theObject, objectData.switches[i].pos,Quaternion.identity,roomParent.transform);
            theObj.GetComponent<Switch>().ChangeState(objectData.switches[i].initState);

            theObj.AddComponent(typeof(EditorObject));
            theObj.GetComponent<EditorObject>().objectId = newSwitch.objectId;
            theObj.GetComponent<EditorObject>().instanceId = objectData.switches[i].instanceId;

            spawnedGameObjects.Add(theObj);

            theRoom.GetComponent<RoomManager>().AddNewSwitch(theObj.GetComponent<Switch>());
        }  
        for(int i = 0; i < objectData.plants.Count; i++)
        {
            RoomObject newPlant = objects.Find(sw => sw.objectId == objectData.plants[i].objectId);
            Debug.Log(newPlant);
            GameObject theObj = Instantiate(newPlant.theObject, objectData.plants[i].pos,Quaternion.identity,roomParent.transform);

            theObj.AddComponent(typeof(EditorObject));
            theObj.GetComponent<EditorObject>().objectId = newPlant.objectId;
            theObj.GetComponent<EditorObject>().instanceId = objectData.plants[i].instanceId;

            spawnedGameObjects.Add(theObj);
            
            theRoom.GetComponent<RoomManager>().AddNewObject(theObj);
        }
        for (int i = 0; i < objectData.buttons.Count; i++)
        {
            RoomObject newButton = objects.Find(sw => sw.objectId == objectData.buttons[i].objectId);
            Debug.Log(newButton);
            GameObject theObj = Instantiate(newButton.theObject, objectData.buttons[i].pos, Quaternion.identity, roomParent.transform);

            theObj.AddComponent(typeof(EditorObject));
            theObj.GetComponent<EditorObject>().objectId = newButton.objectId;
            theObj.GetComponent<EditorObject>().relatedIds = objectData.buttons[i].connectedSwitches;
            theObj.GetComponent<EditorObject>().instanceId = objectData.buttons[i].instanceId;

            ObjectButton buttonComp = theObj.GetComponent<ObjectButton>();
            foreach(int x in objectData.buttons[i].connectedSwitches)
            {
               GameObject newSwitch = spawnedGameObjects.Find(z => z.GetComponent<EditorObject>().instanceId == x);
                if (newSwitch.GetComponent<Switch>() != null)
                    buttonComp.AddNewSwitch(newSwitch.GetComponent<Switch>());
            }

            spawnedGameObjects.Add(theObj);

        }
        for (int i = 0; i < objectData.plates.Count; i++)
        {
            RoomObject newPlate = objects.Find(sw => sw.objectId == objectData.plates[i].objectId);
            Debug.Log(newPlate);
            GameObject theObj = Instantiate(newPlate.theObject, objectData.plates[i].pos, Quaternion.identity, roomParent.transform);

            theObj.AddComponent(typeof(EditorObject));
            theObj.GetComponent<EditorObject>().objectId = newPlate.objectId;
            theObj.GetComponent<EditorObject>().relatedIds = objectData.plates[i].connectedSwitches;
            theObj.GetComponent<EditorObject>().instanceId = objectData.plates[i].instanceId;

            PressureButton plateComp = theObj.GetComponent<PressureButton>();
            foreach (int x in objectData.plates[i].connectedSwitches)
            {
                GameObject newSwitch = spawnedGameObjects.Find(v => v.GetComponent<EditorObject>().instanceId == x);
                if(newSwitch.GetComponent<Switch>() != null)
                plateComp.AddNewSwitch(newSwitch.GetComponent<Switch>());
            }

            spawnedGameObjects.Add(theObj);
        }
        for (int i = 0; i < objectData.finishLines.Count; i++)
        {
            GameObject a = Instantiate(finishLine, objectData.finishPos[i], Quaternion.identity, roomParent.transform);
            spawnedGameObjects.Add(a);

        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ModMode(theRoom);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class EditorManager : MonoBehaviour
{
    public static EditorManager Instance;
    //public List<GameObject> objects = new List<GameObject>();
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera vCamera;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private GameObject cameraTarget;
    [SerializeField]
    private TileMapEditor tileMapEditor;
    [SerializeField]
    private GameObject emptyRoomPrefab;
    [SerializeField]
    private CanvasGroup createRoomButton;

    private GameObject currentRoom;
    private Tilemap currentGroundTileMap;
    private Tilemap currentBackgroundTileMap;

    private List<Room> rooms = new List<Room>();
    public class Room
    {
        public int id;
        public Tilemap GRMap;
        public Tilemap BGMap;
        public GameObject roomObject;
    }

    private void Awake()
    {
        Instance = this;      
    }
    private void Start()
    {
        if (ModLoadedManager.Instance.GetRoomGRMap().Count > 0)
        {
            createRoomButton.alpha = 0;
            createRoomButton.interactable = false;
            tileMapEditor.SetTilemap(rooms[0].GRMap);
            SetRoom(rooms[0].roomObject, rooms[0].GRMap, rooms[0].BGMap);

        }   
    }
    private void Update()
    {
        MoveCamera();
    }

    public void CreateNewRoom()
    {
        GameObject room = Instantiate(emptyRoomPrefab);
        room.name = "room";
        GridManager grid = room.GetComponentInChildren<GridManager>();

        currentGroundTileMap = grid.groundTileMap;
        currentBackgroundTileMap = grid.backgroundTileMap;

        createRoomButton.alpha = 0;
        createRoomButton.interactable = false;

        Room newRoom = new Room();
        newRoom.id = rooms.Count + 1;
        newRoom.GRMap = grid.groundTileMap;
        newRoom.BGMap = grid.backgroundTileMap;
        newRoom.roomObject = room;
        rooms.Add(newRoom);

        MapLoadManager.Instance.theRoom = room;

        tileMapEditor.SetTilemap(rooms[rooms.Count - 1].GRMap);
    }
    public void AddRoomToList(GameObject room,Tilemap groundMap,Tilemap bgMap,int id)
    {
        Room newRoom = new Room();
        newRoom.roomObject = room;
        newRoom.GRMap = groundMap;
        newRoom.BGMap = bgMap;
        newRoom.id = id;
        rooms.Add(newRoom);
    }
    public void SetRoom(GameObject room, Tilemap groundMap, Tilemap bgMap)
    {
        currentRoom = room;
        currentGroundTileMap = groundMap;
        currentBackgroundTileMap = bgMap;
    }
    public Room GetRoom()
    {
        return rooms[0];
    }
    public void ReturnToMainMenu()
    {
        SaveWorld();
        SceneManager.LoadScene(0);
    }
    public void SetCameraSize()
    {
        vCamera.m_Lens.OrthographicSize = slider.value;
    }
    private void SaveWorld()
    {
        foreach(Room x in rooms)
        {
            MapLoadManager.Instance.SaveTileMap(ModLoadedManager.Instance.GetDataPath() + "/" + "room_" + "GR" + x.id.ToString(), currentGroundTileMap);
            MapLoadManager.Instance.SaveTileMap(ModLoadedManager.Instance.GetDataPath() + "/" + "room_" + "BG" + x.id.ToString(), currentBackgroundTileMap);
            MapLoadManager.Instance.SaveRoomData(ModLoadedManager.Instance.GetDataPath() + "/" + "room_" + "OBJ" + x.id.ToString(), MapLoadManager.Instance.spawnedGameObjects);
        }

    }

    private void MoveCamera()
    {
        float speed = 0.25f;
        Vector3 moveDir = new Vector3(0f, 0f, 0f);

        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector3.down;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector3.up;
        }
        if (moveDir.x * moveDir.y != 0)
        {
            cameraTarget.transform.Translate(moveDir * speed / Mathf.Sqrt(2));
        }
        else
        {
            cameraTarget.transform.Translate(moveDir * speed);
        }
    }

}

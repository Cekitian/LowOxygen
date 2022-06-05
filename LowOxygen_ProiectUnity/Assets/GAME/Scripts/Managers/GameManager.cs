using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private AudioClip music;
    [SerializeField] private RoomManager[] rooms;
    [SerializeField] private bool modMode = false;


    private AudioSource musicSource;
    private Transform respawnPoint;
    private RoomManager currentRoom;
    private Player player;
    private void Awake()
    {
        Instance = this;
        musicSource = AudioManager.Instance.PlaySound(music, 0f, 1f, true,1f);
    }
    private void Start()
    {
        player = Player.Instance;

        if (modMode)
            return;

        Debug.Log(rooms.Length);
        for(int i = 0; i <rooms.Length; i++)
        {
            rooms[i].SetIndex(i);
        }
        if (rooms.Length == 1)
            return;

        DisableAllRooms();
        SetCurrentRoom(rooms[GameDataManager.Instance.DATA.currentCheckPoint]);

        RespawnPlayer();
    }
    public AudioSource GetGameMusic()
    {
        return musicSource;
    }
    public void SetCurrentRoom(RoomManager newRoom)
    {
        currentRoom = newRoom;

        currentRoom.gameObject.SetActive(true);
        EnableCloseAdjacentRooms();
        DisableAjacentRooms();

        newRoom.SetInitStats();
        if (currentRoom.GetIndex() >= GameDataManager.Instance.DATA.currentCheckPoint || modMode)
        {
            respawnPoint = currentRoom.GetRespawnPoint();
            if(!modMode)
            GameDataManager.Instance.DATA.currentCheckPoint = currentRoom.GetIndex();

            if (modMode)
                RespawnPlayer();
        }
    }
    public int GetCurrentRoomIndex()
    {
        return currentRoom.GetIndex();
    }
    public void RespawnPlayer()
    {
        Debug.Log(modMode + " RESPAWNING PLAYER");
        Debug.Log(player);
        Debug.Log(respawnPoint);
        if(player != null && respawnPoint != null)
        {
            Debug.Log(modMode + " aaaaa RESPAWNING PLAYER");
            player.transform.position = respawnPoint.position;

        }
        currentRoom.ResetRoomToOriginalState();
    }

    /// <summary>
    /// This makes it so that there are constantly only 3 rooms enabled
    /// The room you are in, the precedent one and the next one
    /// </summary>
    private void DisableAjacentRooms()
    {
        if(currentRoom.GetIndex() >= 3)
        {
            rooms[currentRoom.GetIndex() - 3].gameObject.SetActive(false);
        }
        if(currentRoom.GetIndex() <= rooms.Length - 3)
        {
            rooms[currentRoom.GetIndex() + 2].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Enables the rooms to the left and the right of the current room
    /// </summary>
    private void EnableCloseAdjacentRooms()
    {
        if (currentRoom.GetIndex() >= 1)
        {
            rooms[currentRoom.GetIndex() - 1].gameObject.SetActive(true);
        } 
        if (currentRoom.GetIndex() >= 2)
        {
            rooms[currentRoom.GetIndex() - 2].gameObject.SetActive(true);
        }
        if (currentRoom.GetIndex() <= rooms.Length - 2)
        {
            rooms[currentRoom.GetIndex() + 1].gameObject.SetActive(true);
        }
    }
    private void DisableAllRooms()
    {
        foreach(RoomManager x in rooms)
        {
            x.gameObject.SetActive(false);
        }
    }

    public void ModMode(GameObject room)
    {
        Debug.Log("cox");
        RoomManager roomComp = room.GetComponent<RoomManager>();
        rooms = new RoomManager[1] { roomComp };
        player = Player.Instance;
        respawnPoint = roomComp.GetRespawnPoint();

        Debug.Log(respawnPoint);
        SetCurrentRoom(roomComp);
        RespawnPlayer();

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private RoomManager[] rooms;


    private Transform respawnPoint;
    private RoomManager currentRoom;
    private Player player;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        player = Player.Instance;

        for(int i = 0; i <rooms.Length; i++)
        {
            rooms[i].SetIndex(i);
        }
        if (rooms.Length == 0)
            return;
        DisableAllRooms();
        SetCurrentRoom(rooms[GameDataManager.Instance.DATA.currentCheckPoint]);

        RespawnPlayer();
    }
    public void SetCurrentRoom(RoomManager newRoom)
    {
        currentRoom = newRoom;

        currentRoom.gameObject.SetActive(true);
        EnableCloseAdjacentRooms();
        DisableAjacentRooms();

        if (currentRoom.GetIndex() >= GameDataManager.Instance.DATA.currentCheckPoint)
        {
            respawnPoint = currentRoom.GetRespawnPoint();
            GameDataManager.Instance.DATA.currentCheckPoint = currentRoom.GetIndex();
        }
        newRoom.SetInitStats();
    }
    public int GetCurrentRoomIndex()
    {
        return currentRoom.GetIndex();
    }
    public void RespawnPlayer()
    {
        if(player != null && respawnPoint != null)
        player.transform.position = respawnPoint.position;
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
        RoomManager roomComp = room.GetComponent<RoomManager>();
        rooms = new RoomManager[1] { roomComp };

        SetCurrentRoom(roomComp);
        RespawnPlayer();

    }


}

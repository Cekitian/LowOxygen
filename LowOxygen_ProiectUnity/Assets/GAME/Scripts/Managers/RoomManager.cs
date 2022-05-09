using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;
    [SerializeField]
    private GameObject[] objects; 
    [SerializeField]
    private Switch[] switches;


    private int roomIndex;
    private Vector3[] initialPos;
    private bool[] initialState;
    private List<Plant_Grow> growPlants = new List<Plant_Grow>();
    private void Awake()
    {
        initialPos = new Vector3[objects.Length];
        initialState = new bool[switches.Length];
        foreach(GameObject x in objects)
        {
            if(x.TryGetComponent(out Plant_Grow y))
            {
                growPlants.Add(y);
            }
        }


        SetInitStats();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
           // if(GameManager.Instance.GetCurrentRoomIndex() < roomIndex)
            {
                GameManager.Instance.SetCurrentRoom(this);
            }
        }
    }
    public Transform GetRespawnPoint()
    {
        return respawnPoint;
    }
    public void ResetRoomToOriginalState()
    {
        foreach (Plant_Grow x in growPlants)
        {
            x.ResetGrowState();
        }

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = initialPos[i];

            if(objects[i].TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
            }
        } 
        for(int i = 0; i < switches.Length; i++)
        {
            switches[i].ChangeState(initialState[i]);
        }       
    }
    public void SetIndex(int newIndex)
    {
        roomIndex = newIndex;
    }
    public int GetIndex()
    {
        return roomIndex; ;
    }
    private void SetInitStats()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            initialPos[i] = objects[i].transform.position;
        }
        for (int i = 0; i < switches.Length; i++)
        {
            initialState[i] = switches[i].GetState();
        }
    }

}

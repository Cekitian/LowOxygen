using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public Oxygen_State oxygenState;
    public Player_Controller playerController;
    public Player_Collision playerCollision;
    public Player_Dialogue playerDialogue;
    public Player_Hats playerHats;
    [Space]
    public GameObject frogHat;
    [Space]
    [Space]
    public Plant nearbyPlant;
    [Space]
    [SerializeField] private float oxygenDuration;
    [TextArea(2,4)] [SerializeField] private string[] deathMessages;
    [TextArea(2,4)] [SerializeField] private string[] restartMessages;

    private float timePlayerDies;
    private bool alive = true;

    private void Awake()
    {
        Instance = this;
        oxygenState.ChangedOxygenState += ChangedOxygenState;       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (!alive)
            Debug.Log("PLAYER DIED");

        if (oxygenState.oxygenState == Oxygen_State.OXYGEN.OXYGEN)
            return;


        if(Time.time >= timePlayerDies && alive)
        {
            alive = false;
        }      
    }
    private void ChangedOxygenState()
    {
        if(oxygenState.oxygenState == Oxygen_State.OXYGEN.NO_OXYGEN)
        {
            timePlayerDies = Time.time + oxygenDuration;
        }
    }
    public void SetPlant(Plant plant)
    {
        nearbyPlant = plant;
    }

    public void Die()
    {
        playerDialogue.DisplayDialogue(deathMessages[Random.Range(0, deathMessages.Length)], 5);
        GameManager.Instance.RespawnPlayer();
    }
    public void Restart()
    {
        playerDialogue.DisplayDialogue(restartMessages[Random.Range(0, restartMessages.Length)], 5);
        GameManager.Instance.RespawnPlayer();
    }
}

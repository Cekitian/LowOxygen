using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectButton : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [Space]
    [SerializeField] private Switch[] switches;
    [SerializeField] private Color buttonColor;
    private bool playerInside;

    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().color = buttonColor;

        foreach (Switch x in switches)
        {
            if(x.GetType() == typeof(Switch_Door))
            {
                Debug.Log("I have a door");
                x.gameObject.GetComponent<Switch_Door>().doorColor = buttonColor;
            }
            else if (x.GetType() == typeof(MovingPlatform))
            {
                Debug.Log("I have a door");
                x.gameObject.GetComponent<MovingPlatform>().SetColor(buttonColor);
            }
        }
    }
    private void Update()
    {
        if (!playerInside)
            return;

        if(Input.GetKeyDown(PlayerInputKeys.interact))
        {
            AudioManager.Instance.PlaySound(buttonSound, 0.5f, 1.5f, false);

            foreach (Switch x in switches)
                x.ChangeState();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        playerInside = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        playerInside = false;    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [Space]
    [SerializeField] private Color buttonColor;
    [SerializeField]
    private bool pressedValue = true;  
    [SerializeField]
    private int pressureNeeded;
    [SerializeField]
    private Switch[] switches;
    private List<GameObject> objectsOnButton = new List<GameObject>();

    private bool beingPressed = false;

    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().color = buttonColor;
        foreach (Switch x in switches)
        {    
            if (x.GetType() == typeof(Switch_Door))
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Plant"))
        {
            objectsOnButton.Add(collision.gameObject);
        }
        if(objectsOnButton.Count >= pressureNeeded && !beingPressed)
        {
            beingPressed = true;
            AudioManager.Instance.PlaySound(buttonSound, 0.5f, 1.75f, false);

            foreach (Switch x in switches)
            {
                x.ChangeState(pressedValue);
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Plant"))
        {
            objectsOnButton.Remove(collision.gameObject);

        }
        if (objectsOnButton.Count < pressureNeeded)
        {
            beingPressed = false;

            foreach (Switch x in switches)
            {
                x.ChangeState(!pressedValue);
            }
        }
    }

    public void AddNewSwitch(Switch newSwitch)
    {
        Switch[] copy = new Switch[switches.Length + 1];
        for (int i = 0; i < copy.Length - 1; i++)
        {
            copy[i] = switches[i];
        }
        copy[copy.Length - 1] = newSwitch;
        switches = copy;
    }
    public Switch[] GetSwitches()
    {
        return switches;
    }
}

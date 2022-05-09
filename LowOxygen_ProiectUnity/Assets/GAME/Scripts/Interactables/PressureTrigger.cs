using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureTrigger : MonoBehaviour
{
    [SerializeField]
    private int pressureNeeded;
    [SerializeField]
    private Switch[] switches;
    private List<GameObject> objectsOnButton = new List<GameObject>();

    private bool triggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered)
            return;

        if (collision.CompareTag("Player") || collision.CompareTag("Plant"))
        {
            objectsOnButton.Add(collision.gameObject);
        }
        if (objectsOnButton.Count >= pressureNeeded)
        {
            foreach (Switch x in switches)
            {
                x.ChangeState(true);
                triggered = true;
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
            foreach (Switch x in switches)
            {
                x.ChangeState(false);
            }
        }
    }
}

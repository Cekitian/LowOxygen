using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Switch
{
    public Color platformColor;

    [SerializeField]
    private bool constantMove;
    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;  
    [SerializeField]
    private GameObject platform;
    [SerializeField]
    private float time;

    private float index = 0;

    private List<GameObject> objectsOnPlatform = new List<GameObject>();
    private bool movingToA;
    protected override void Init()
    {
        if (!currentState)
        {
            platform.transform.position = pointA.transform.position;
            index = 1;
            movingToA = true;
        }
        else
        {
            platform.transform.position = pointB.transform.position;
            index = 0;
            movingToA = false;
        }

    }
    protected override void DoOnStateChange()
    {
        if (!currentState)
        {
            if (movingToA && index >= 1)
            {
                movingToA = false;
                currentState = !currentState;
                index = 1;
            }
        }
        else
        {
            if (!movingToA && index <= 0)
            {
                movingToA = true;
                currentState = !currentState;
                index = 0;
            }
        }
    }

    public override void ChangeState()
    {      
        DoOnStateChange();
    }
    //forces the platform to move to one of the poles
    // also removes player from its list
    public override void ChangeState(bool newState)
    {
        objectsOnPlatform = new List<GameObject>();
        currentState = newState;

        if(!currentState)
        {
            movingToA = true;
            index = 1;  
        }
        else
        {
            movingToA = false;
            index = 0;
        }
    }
    public void SetColor(Color newColor)
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = newColor;
    }
    private void FixedUpdate()
    {
        var prevPos = platform.transform.position;

        if(!currentState)
        {
            platform.transform.position = Vector3.Lerp(pointB.position, pointA.position, index);
            index += Time.fixedDeltaTime / time;
            if (index >= 1 && constantMove)
            {
                currentState = true;
            }
        }
        else if(currentState)
        {
            platform.transform.position = Vector3.Lerp(pointB.position, pointA.position, index);
            index -= Time.fixedDeltaTime / time;

            if (index <= 0 && constantMove)
            {
                currentState = false;
            }
        }

        var posDifference = platform.transform.position - prevPos;
        foreach(GameObject x in objectsOnPlatform)
        {
            x.transform.position += posDifference;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Plant"))
        {
            if(!collision.isTrigger)
            objectsOnPlatform.Add(collision.gameObject);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Plant"))
        {
            if(!collision.isTrigger)
            objectsOnPlatform.Remove(collision.gameObject);

        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Door : Switch
{
    public Color doorColor;

    private Collider2D doorCollider;
    private SpriteRenderer doorRenderer;

    protected override void Init()
    {
        doorCollider = gameObject.GetComponent<Collider2D>();
        doorRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        if(currentState)
        {
            //if door is open
            doorCollider.enabled = false;
            doorRenderer.color =  doorColor * Color.gray;
        }
        else
        {
            //if door is closed
            doorCollider.enabled = true;
            doorRenderer.color = doorColor;
        }
    }
    protected override void DoOnStateChange()
    {
        if (currentState)
        {
            //if door is open
            doorCollider.enabled = false;
            doorRenderer.color = doorColor * Color.gray;
        }
        else 
        {
            //if door is closed
            doorCollider.enabled = true;
            doorRenderer.color = doorColor;
        }
    }

}

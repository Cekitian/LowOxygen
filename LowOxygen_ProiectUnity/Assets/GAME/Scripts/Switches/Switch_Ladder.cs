using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Ladder : Switch
{
   [SerializeField] private BoxCollider2D ladderTrigger;
   [SerializeField] private SpriteRenderer ladderRenderer;
    protected override void Init()
    {
        if (currentState)
        {
            ladderRenderer.color = Color.white;
            ladderTrigger.enabled = true;
        }
        else
        {
            ladderRenderer.color = Color.gray;
            ladderTrigger.enabled = false;
        }
    }
    protected override void DoOnStateChange()
    {
        if(currentState)
        {
            ladderRenderer.color = Color.white;
            ladderTrigger.enabled = true;
        }   
        else
        {
            ladderRenderer.color = Color.gray;
            ladderTrigger.enabled = false;
        }

    }
 
}

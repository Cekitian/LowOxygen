using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    [SerializeField] private bool triggerOnce;
    [TextArea(2,4)] [SerializeField] private string dialogue;
    [SerializeField] private float duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Instance.playerDialogue.DisplayDialogue(dialogue, duration);

        if (triggerOnce)
            Destroy(gameObject);
    }
}

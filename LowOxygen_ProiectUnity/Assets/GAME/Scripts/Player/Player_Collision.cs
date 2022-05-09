using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collision : MonoBehaviour
{
    public bool onLadder;
    public CapsuleCollider2D playerCollider;

    private List<Collider2D> ladders = new List<Collider2D>();
    private Animator animator;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Plant"))
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>(), true);
            collision.collider.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladders.Add(collision);
            onLadder = true;

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        { 
            if(Player.Instance.playerController.IsGrabbing())
            {
                animator.SetBool("Climbing", false);
            }
            else
            {
                animator.SetBool("Climbing", true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladders.Remove(collision);
            if(ladders.Count == 0)
            {
                onLadder = false;

                animator.SetBool("Climbing", false);
            }
            
        }
    }
}

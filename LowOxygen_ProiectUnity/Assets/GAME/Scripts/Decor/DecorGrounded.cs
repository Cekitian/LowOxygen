using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorGrounded : MonoBehaviour
{
   [SerializeField] private bool isCeilling = false;
   [SerializeField] private LayerMask groundedLayer;

    private BoxCollider2D decorCollider;
    private void Awake()
    {
        decorCollider = gameObject.GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        if(isCeilling)
        {
            RaycastHit2D ray = Physics2D.Raycast(decorCollider.transform.position + Vector3.up * decorCollider.offset.y + Vector3.up * decorCollider.size.y / 2, Vector2.up, Mathf.Infinity, groundedLayer);

            if (ray.collider != null)
            {
                gameObject.transform.position += Vector3.up * ray.distance;
            }
        }
        else
        {
            RaycastHit2D ray = Physics2D.Raycast(decorCollider.transform.position + Vector3.up * decorCollider.offset.y - Vector3.up * decorCollider.size.y / 2, Vector2.down, Mathf.Infinity, groundedLayer);

            if (ray.collider != null)
            {
                gameObject.transform.position -= Vector3.up * ray.distance;
            }
        }
       
    }
}

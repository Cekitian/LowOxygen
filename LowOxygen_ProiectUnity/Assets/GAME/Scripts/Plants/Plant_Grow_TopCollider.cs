using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Grow_TopCollider : MonoBehaviour
{
    private Collider2D theCollider;

    private List<GameObject> groundHit = new List<GameObject>();
    private void Awake()
    {
        theCollider = gameObject.GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)//if its considered ground
        {
            gameObject.GetComponentInParent<Plant_Grow>().ChangeGrowState(false);
            Debug.Log("HIT CEILING");
            if(!groundHit.Exists(x => x == collision.gameObject))
            {
                groundHit.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)//if its considered ground
        {
            groundHit.Remove(collision.gameObject);

            if(groundHit.Count == 0)
            {
                gameObject.GetComponentInParent<Plant_Grow>().ChangeGrowState(true);
            }

        }
    }
}

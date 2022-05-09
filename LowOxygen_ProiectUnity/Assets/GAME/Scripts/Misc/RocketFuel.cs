using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFuel : MonoBehaviour
{
    public bool startedFueling = false;

    [SerializeField] private GameObject gas;
    [SerializeField] private Transform fuelPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player") && !startedFueling)
        {
            startedFueling = true;
            StartCoroutine(StartFueling());
        }
    }
    private IEnumerator StartFueling()
    {
        //visual animation;

        for(float i = - 1.5f; i<= 1.5f; i++)
        {
            GameObject newGas = Instantiate(gas, fuelPoint.transform.position + Vector3.right * i * 6 + Vector3.up * 4, Quaternion.identity);
            newGas.GetComponent<GasCollectible>().followTarget = fuelPoint;
            newGas.GetComponent<GasCollectible>().notFollowPlayer = true;
            Debug.Log("Spawned fuel");

            yield return new WaitForSeconds(0.25f);
        }

        yield break;
    }
}

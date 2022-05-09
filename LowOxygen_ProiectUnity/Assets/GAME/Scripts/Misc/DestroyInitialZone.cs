using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInitialZone : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("MADE INACTIVE");
        StartCoroutine(DestroyObject());
    }
    private void Update()
    {
        StartCoroutine(DestroyObject());
    }
    private IEnumerator DestroyObject()
    {
        Debug.Log("MADE INACTIVE");
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        yield break;
    }
}

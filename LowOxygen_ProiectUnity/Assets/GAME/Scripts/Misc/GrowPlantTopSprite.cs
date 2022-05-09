using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlantTopSprite : MonoBehaviour
{
    [SerializeField]
    private GameObject growPart;
    private Vector3 initPos;
    private void Awake()
    {
        initPos = gameObject.transform.localPosition;
    }
    private void Update()
    {
        gameObject.transform.localPosition = initPos + (growPart.transform.localScale - Vector3.one);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledSpriteScale : MonoBehaviour
{
    [SerializeField]
    private GameObject objectRef;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Vector3 refScalePoint;
    [SerializeField]
    private Vector3 scaleOffset;
    private void Awake()
    {
        refScalePoint = Vector3.zero;
    }
    private void Update()
    {
        if (objectRef == null || spriteRenderer == null)
            return;

        spriteRenderer.size = objectRef.transform.localScale - scaleOffset;
        spriteRenderer.transform.position = objectRef.transform.position + refScalePoint;
    }
}

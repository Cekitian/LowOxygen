using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hats : MonoBehaviour
{
    public Sprite[] hatSprites;

    [SerializeField] private GameObject hatParent;
    private int hatsPlaced = 0;
    private float distanceBetween = 0.45f;
    private void Awake()
    {
       for(int i = 0; i < GameDataManager.Instance.DATA.hasHat.Length; i++)
        {
            if (GameDataManager.Instance.DATA.hasHat[i])
            {
                AddHat(i);
            }
        }
    }
    
    public void AddHat(int index)
    {

        GameObject newHat = new GameObject();
        newHat.transform.parent = hatParent.transform;
        newHat.transform.localPosition = Vector3.zero + Vector3.up * hatsPlaced * distanceBetween;
        newHat.transform.localScale = Vector3.one * 1.2f;//to be slightly bigger

        newHat.AddComponent(typeof(SpriteRenderer));
        var renderer = newHat.GetComponent<SpriteRenderer>();
        renderer.sprite = hatSprites[index];
        renderer.sortingLayerName = "CenterNoLight";
        renderer.sortingOrder = 3;
        
        hatsPlaced++;
    }
    public SpriteRenderer[] GetRenderers()
    {
        return hatParent.GetComponentsInChildren<SpriteRenderer>();
    }
}

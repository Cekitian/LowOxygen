using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tiles/New Tile")]
public class TileObject : ScriptableObject
{
    public string id;
    public TileBase theTile;
}

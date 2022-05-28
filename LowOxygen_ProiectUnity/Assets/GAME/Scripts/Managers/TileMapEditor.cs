using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapEditor : MonoBehaviour
{
    public bool editorActive = false;
    [SerializeField] private ConstructionManager constructionEditor;
    [SerializeField] private LogicManager logicEditor;
    [SerializeField] private CanvasGroup tileCanvas;
    [SerializeField] private Tilemap currentTileMap;
    [SerializeField] private TileBase currentTile;
    [Space]
    [SerializeField] public GameObject groundTiles;
    [SerializeField] private GameObject backgroundTiles;
    [Space]
    [SerializeField] private TileObject[] tiles;
    [SerializeField] private Camera cam;

    private Vector3Int lastTilePlaced;

    private void LateUpdate()
    {
        if (!editorActive)
            return;

        Vector3Int mousePos = currentTileMap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButton(0))
        {
            PlaceTile(mousePos);
        }
        if(Input.GetMouseButton(1))
        {
            RemoveTile(mousePos);
        }
    }

    public void SetTilemap(Tilemap newTileMap)
    {
        DestroyedLastPlaced();
        currentTileMap = newTileMap;

    }
    public void ChangeSelectedTile(int index)
    {
        DestroyedLastPlaced();
        currentTile = tiles[index].theTile;
        if(index >= tiles.Length / 2)
        {
            currentTileMap = EditorManager.Instance.GetRoom().BGMap; 
        }
        else
        {
            currentTileMap = EditorManager.Instance.GetRoom().GRMap; 
        }
    }
    public void ChangeTileMode()
    {
        DestroyedLastPlaced();
        if(constructionEditor.editorActive)
        {
            constructionEditor.ChangeActiveState();
        }
        if (logicEditor.editorActive)
            logicEditor.ChangeActiveState();

        if (editorActive == false)
        {
            tileCanvas.alpha = 1;
            tileCanvas.interactable = true;

            editorActive = true;
        }
        else
        {
            tileCanvas.alpha = 0;
            tileCanvas.interactable = false;

            editorActive = false;
        }
       
    }
    public void ChangeTypeOfTiles(int index)
    {
        DestroyedLastPlaced();
        if (index == 0)
        {
            groundTiles.SetActive(true);
            backgroundTiles.SetActive(false);
        }
        else
        {
            groundTiles.SetActive(false);
            backgroundTiles.SetActive(true);
        }
    }
    private void PlaceTile(Vector3Int pos)
    {
        pos.z = 0;
        currentTileMap.SetTile(pos, currentTile);
        lastTilePlaced = pos;
    }
    private void RemoveTile(Vector3Int pos)
    {
        pos.z = 0;
        currentTileMap.SetTile(pos, null);
    }    
    private void DestroyedLastPlaced()
    {
        if(lastTilePlaced != null && currentTileMap != null)
        currentTileMap.SetTile(lastTilePlaced, null);
    }


}

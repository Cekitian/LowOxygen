using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public bool editorActive = false;

    [SerializeField] private TileMapEditor tileEditor;
    [SerializeField] private CanvasGroup constructionGroup;
    [SerializeField] private LogicManager logicEditor;
    [SerializeField] private RoomObject currentObject;
    [SerializeField] private Camera cam;
    [Space]
    [SerializeField] private GameObject firstMenu;
    [SerializeField] private GameObject secondMenu;
    [SerializeField] private GameObject thirdMenu;
    [Space]
    [SerializeField] private RoomObject[] objectsToBePlaced;

    private GameObject lastObjectPlaced;
    private void LateUpdate()
    {
        if (!editorActive)
            return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Spawned Plant");
            PlaceObject(mousePos);
        } 
        if(Input.GetMouseButtonDown(1))
        {
            DestroyObject(mousePos);
        }
        if(Input.GetMouseButtonDown(2))
        {
            Debug.Log("SWITCH");
            ChangeSwitchState(mousePos);
        }
    }
    public void ChangeActiveState()
    {
        DestroyLastObjectPlaced();

        if (tileEditor.editorActive)
        {
            tileEditor.ChangeTileMode();
        }
        if(logicEditor.editorActive)
        logicEditor.ChangeActiveState();

        if(editorActive)
        {
            constructionGroup.alpha = 0;
            constructionGroup.interactable = false;
            editorActive = false;
        }
        else
        {
            constructionGroup.alpha = 1;
            constructionGroup.interactable = true;
            editorActive = true;
        }
    }
    public void PlaceObject(Vector3 pos)
    {
        pos.z = 0;
        Debug.Log(currentObject);
        GameObject newObj = Instantiate(currentObject.theObject, pos, Quaternion.identity, MapLoadManager.Instance.theRoom.transform);
        newObj.AddComponent(typeof(EditorObject));
        newObj.GetComponent<EditorObject>().objectId = currentObject.objectId;
        MapLoadManager.Instance.spawnedGameObjects.Add(newObj);
        newObj.GetComponent<EditorObject>().instanceId = MapLoadManager.Instance.spawnedGameObjects.Count;
        lastObjectPlaced = newObj;

    }
    public void DestroyLastObjectPlaced()
    {
        if(lastObjectPlaced != null)
        {
            MapLoadManager.Instance.spawnedGameObjects.Remove(lastObjectPlaced);
            Destroy(lastObjectPlaced);
        }
      
    }
    public void DestroyObject(Vector3 pos)
    {
        pos.z = 0;
        for(int i = 0; i < MapLoadManager.Instance.spawnedGameObjects.Count; i++)
        {
            if(Vector3.Distance(MapLoadManager.Instance.spawnedGameObjects[i].transform.position,pos) < 1)
            {
                GameObject obj = MapLoadManager.Instance.spawnedGameObjects[i];
                MapLoadManager.Instance.spawnedGameObjects.Remove(MapLoadManager.Instance.spawnedGameObjects[i]);
                Destroy(obj);
                break;
            }
        }
    }
    public void ChangeSwitchState(Vector3 pos)
    {
        pos.z = 0;
        for (int i = 0; i < MapLoadManager.Instance.spawnedGameObjects.Count; i++)
        {
            if (Vector3.Distance(MapLoadManager.Instance.spawnedGameObjects[i].transform.position, pos) < 1)
            {
                if(MapLoadManager.Instance.spawnedGameObjects[i].TryGetComponent(out Switch y))
                {
                    y.ChangeState();
                }

            }
        }
    }
    public void ChangeShownMenu(int menuIndex)
    {
        DestroyLastObjectPlaced();

        switch (menuIndex)
        {
            case 0:
                firstMenu.SetActive(true);
                secondMenu.SetActive(false);
                thirdMenu.SetActive(false);
                break;
            case 1:
                firstMenu.SetActive(false);
                secondMenu.SetActive(true);
                thirdMenu.SetActive(false);
                break;
            case 2:
                firstMenu.SetActive(false);
                secondMenu.SetActive(false);
                thirdMenu.SetActive(true);
                break;
        }

    }
    public void ChangePlaceableObject(int objectIndex)
    {
        DestroyLastObjectPlaced();
        currentObject = objectsToBePlaced[objectIndex];
    }

}

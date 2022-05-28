using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public bool editorActive = false;

    [SerializeField] private Camera cam;
    [SerializeField] private TileMapEditor tileEditor;
    [SerializeField] private ConstructionManager constructionEditor;
    [SerializeField] private GameObject selectedCircle;

    private ObjectButton selectedButton;
    private PressureButton selectedPlate;
    private List<GameObject> selectedCircles = new List<GameObject>();
    private void LateUpdate()
    {
        if (!editorActive)
            return;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            CheckForButton(mousePos);
            CheckForSwitch(mousePos);
        }
    }
    public void ChangeActiveState()
    {
        if(tileEditor.editorActive)
        {
            tileEditor.ChangeTileMode();
        }
        if(constructionEditor.editorActive)
        {
            constructionEditor.ChangeActiveState();
        }
        
        if(editorActive)
        {
            editorActive = false;
            selectedButton = null;
            selectedPlate = null;
        }
        else
        {
            editorActive = true;
        }    
    }
    public void CheckForButton(Vector3 pos)
    {
        pos.z = 0;
        for (int i = 0; i < MapLoadManager.Instance.spawnedGameObjects.Count; i++)
        {
            if (Vector3.Distance(MapLoadManager.Instance.spawnedGameObjects[i].transform.position, pos) < 1)
            {
               if(MapLoadManager.Instance.spawnedGameObjects[i].TryGetComponent(out ObjectButton button))
                {
                    Debug.Log("SELECTED BUTTON");
                    selectedButton = button;
                    selectedPlate = null;

                    DestroyCircles();

                    SpawnCircle(button.gameObject.transform.position);
                    foreach(Switch x in button.GetSwitches())
                    {
                        SpawnCircle(x.gameObject.transform.position);
                    }

                }else
                if (MapLoadManager.Instance.spawnedGameObjects[i].TryGetComponent(out PressureButton press))
                {
                    Debug.Log("SELECTED PLATE");
                    selectedPlate = press;
                    selectedButton = null;

                    DestroyCircles();

                    SpawnCircle(press.gameObject.transform.position);
                    foreach (Switch x in press.GetSwitches())
                    {
                        SpawnCircle(x.gameObject.transform.position);
                    }
                }
            }
        }
    }
    public void CheckForSwitch(Vector3 pos)
    {
        pos.z = 0;
        for (int i = 0; i < MapLoadManager.Instance.spawnedGameObjects.Count; i++)
        {
            if (Vector3.Distance(MapLoadManager.Instance.spawnedGameObjects[i].transform.position, pos) < 1)
            {
                if (MapLoadManager.Instance.spawnedGameObjects[i].TryGetComponent(out Switch y))
                {
                    Debug.Log("ADDING NEW SWITCH");
                    AddSwitchToButton(y);

                }

            }
        }
    }
    public void AddSwitchToButton(Switch theSwitch)
    {
        if (selectedButton == null && selectedPlate == null)
            return;

        SpawnCircle(theSwitch.gameObject.transform.position);
        if (selectedPlate != null)
        {
            if (!selectedPlate.GetComponent<EditorObject>().relatedIds.Exists(t => t == theSwitch.GetComponent<EditorObject>().instanceId))
            {
                selectedPlate.GetComponent<EditorObject>().relatedIds.Add(theSwitch.GetComponent<EditorObject>().instanceId);
                selectedPlate.AddNewSwitch(theSwitch);
            }     
        }
        else if(selectedButton != null)
        {
            if (!selectedButton.GetComponent<EditorObject>().relatedIds.Exists(v => v == theSwitch.GetComponent<EditorObject>().instanceId))
            {
                selectedButton.GetComponent<EditorObject>().relatedIds.Add(theSwitch.GetComponent<EditorObject>().instanceId);
                selectedButton.AddNewSwitch(theSwitch);
            }  
        }
    }

    private void SpawnCircle(Vector3 pos)
    {
        GameObject Instance = Instantiate(selectedCircle, pos,Quaternion.identity);
        selectedCircles.Add(Instance);
    }
    private void DestroyCircles()
    {
        foreach(GameObject x in selectedCircles)
        {
            Destroy(x);
        }
        selectedCircles = new List<GameObject>();
    }
}

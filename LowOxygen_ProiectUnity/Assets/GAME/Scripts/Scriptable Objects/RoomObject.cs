using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Object", menuName = "Objects/New Object")]
public class RoomObject : ScriptableObject
{
    public string objectId;
    public GameObject theObject;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Decor", menuName = "Decors/new Decor")]
public class DecorObject : ScriptableObject
{
    public string decorId;
    public GameObject decor;
}

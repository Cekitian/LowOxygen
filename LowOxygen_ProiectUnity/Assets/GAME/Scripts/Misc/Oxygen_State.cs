using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen_State : MonoBehaviour
{
    public OXYGEN oxygenState;

    public System.Action ChangedOxygenState;
   public enum OXYGEN
    {
        OXYGEN = 0, NO_OXYGEN = 1
    }

    public void ChangeState(OXYGEN newState)
    {
        oxygenState = newState;
        ChangedOxygenState?.Invoke();
        Debug.Log("CHANGED OXYGEN");
    }
}

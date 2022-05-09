using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch : MonoBehaviour
{
    [SerializeField]
    protected bool currentState;

    private void Awake()
    {
        Init();
    }  
    private void OnEnable()
    {
        Init();
    }
    protected abstract void Init();
    protected abstract void DoOnStateChange();
    public bool GetState()
    {
        return currentState;
    }
    public virtual void ChangeState()
    {
        currentState = !currentState;
        DoOnStateChange();
        
    }
    public virtual void ChangeState(bool newState)
    {
        currentState = newState;
        DoOnStateChange();
    }
}

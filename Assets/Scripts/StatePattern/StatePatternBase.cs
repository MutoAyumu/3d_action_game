using System.Collections.Generic;
using UnityEngine;

public class StatePatternBase : MonoBehaviour
{
    protected StateBase _currentState;

    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        OnStart();
        _currentState.OnEnter(this, null);
    }
    private void Update()
    {
        OnUpdate();
        _currentState.OnUpdate(this);
    }

    /// <summary>
    /// StateÇÃêÿÇËë÷Ç¶ÇÇ∑ÇÈ
    /// </summary>
    /// <param name="nextState"></param>
    public void ChangeState(StateBase nextState)
    {
        _currentState.OnExit(this, nextState);
        nextState.OnEnter(this, _currentState);
        _currentState = nextState;
        Debug.Log($"CurrentState {_currentState} : NextState {nextState}");
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
}
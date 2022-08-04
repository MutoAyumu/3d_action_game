using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    static readonly PlayerMoveState _moveState = new PlayerMoveState();

    StateBase _currentState = _moveState;

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
    /// State‚ÌØ‚è‘Ö‚¦‚ğ‚·‚é
    /// </summary>
    /// <param name="nextState"></param>
    public void ChangeState(StateBase nextState)
    {
        _currentState.OnExit(this, nextState);
        nextState.OnEnter(this, _currentState);
        _currentState = nextState;
    }
}
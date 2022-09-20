using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    //�e�X�e�[�g�𐶐�
    static readonly PlayerMoveState _moveState = new PlayerMoveState();
    static readonly PlayerJumpState _jumpState = new PlayerJumpState();
    static readonly PlayerGrapplingState _grapplingState = new PlayerGrapplingState();

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
    /// State�̐؂�ւ�������
    /// </summary>
    /// <param name="nextState"></param>
    public void ChangeState(StateBase nextState)
    {
        _currentState.OnExit(this, nextState);
        nextState.OnEnter(this, _currentState);
        _currentState = nextState;
        Debug.Log($"CurrentState {_currentState} : NextState {nextState}");
    }
}
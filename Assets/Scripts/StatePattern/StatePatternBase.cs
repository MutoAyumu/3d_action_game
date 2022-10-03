using System.Collections.Generic;
using UnityEngine;

public class StatePatternBase<TOwner>
{
    public abstract class StateBase
    {
        public StatePatternBase<TOwner> StatePattern;
        protected TOwner Owner => StatePattern.Owner;

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }

    TOwner Owner { get; }
    StateBase _currentState;
    StateBase _prevState;
    readonly Dictionary<int, StateBase> _states = new Dictionary<int, StateBase>();

    public StatePatternBase(TOwner owner)
    {
        Owner = owner;
    }

    public void Add<T>(int stateId) where T : StateBase, new()
    {
        if (_states.ContainsKey(stateId))
        {
            Debug.LogError("already register stateId!! : " + stateId);
            return;
        }
        // �X�e�[�g��`��o�^
        var newState = new T
        {
            StatePattern = this
        };
        _states.Add(stateId, newState);
    }

    /// <summary>
    /// �X�e�[�g�J�n����
    /// </summary>
    /// <param name="stateId">�X�e�[�gID</param>
    public void OnStart(int stateId)
    {
        if (!_states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError("not set stateId!! : " + stateId);
            return;
        }
        // ���݂̃X�e�[�g�ɐݒ肵�ď������J�n
        _currentState = nextState;
        _currentState.OnEnter();
    }

    /// <summary>
    /// �X�e�[�g�X�V����
    /// </summary>
    public void OnUpdate()
    {
        _currentState.OnUpdate();
    }

    /// <summary>
    /// ���̃X�e�[�g�ɐ؂�ւ���
    /// </summary>
    /// <param name="stateId">�؂�ւ���X�e�[�gID</param>
    public void ChangeState(int stateId)
    {
        if (!_states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError("not set stateId!! : " + stateId);
            return;
        }

        Debug.Log($"CurrentState {_currentState} : NextState {nextState}");

        // �O�̃X�e�[�g��ێ�
        _prevState = _currentState;
        // �X�e�[�g��؂�ւ���
        _currentState.OnExit();
        _currentState = nextState;
        _currentState.OnEnter();
    }

    /// <summary>
    /// �O��̃X�e�[�g�ɐ؂�ւ���
    /// </summary>
    public void ChangePrevState()
    {
        if (_prevState == null)
        {
            Debug.LogError("prevState is null!!");
            return;
        }
        // �O�̃X�e�[�g�ƌ��݂̃X�e�[�g�����ւ���
        (_prevState, _currentState) = (_currentState, _prevState);
    }
}
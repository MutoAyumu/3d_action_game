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
        // ステート定義を登録
        var newState = new T
        {
            StatePattern = this
        };
        _states.Add(stateId, newState);
    }

    /// <summary>
    /// ステート開始処理
    /// </summary>
    /// <param name="stateId">ステートID</param>
    public void OnStart(int stateId)
    {
        if (!_states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError("not set stateId!! : " + stateId);
            return;
        }
        // 現在のステートに設定して処理を開始
        _currentState = nextState;
        _currentState.OnEnter();
    }

    /// <summary>
    /// ステート更新処理
    /// </summary>
    public void OnUpdate()
    {
        _currentState.OnUpdate();
    }

    /// <summary>
    /// 次のステートに切り替える
    /// </summary>
    /// <param name="stateId">切り替えるステートID</param>
    public void ChangeState(int stateId)
    {
        if (!_states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError("not set stateId!! : " + stateId);
            return;
        }

        Debug.Log($"CurrentState {_currentState} : NextState {nextState}");

        // 前のステートを保持
        _prevState = _currentState;
        // ステートを切り替える
        _currentState.OnExit();
        _currentState = nextState;
        _currentState.OnEnter();
    }

    /// <summary>
    /// 前回のステートに切り替える
    /// </summary>
    public void ChangePrevState()
    {
        if (_prevState == null)
        {
            Debug.LogError("prevState is null!!");
            return;
        }
        // 前のステートと現在のステートを入れ替える
        (_prevState, _currentState) = (_currentState, _prevState);
    }
}
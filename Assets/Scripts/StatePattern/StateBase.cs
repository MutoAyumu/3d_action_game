using UnityEngine;

/// <summary>
/// 各Stateのベースクラス
/// </summary>
public abstract class StateBase
{
    public virtual void OnEnter(StatePatternBase entity, StateBase state) { }

    public virtual void OnUpdate(StatePatternBase entity) { }

    public virtual void OnExit(StatePatternBase entity, StateBase nextState) { }

    public virtual void OnInit(StatePatternBase entity) { }
}

using UnityEngine;

/// <summary>
/// �eState�̃x�[�X�N���X
/// </summary>
public abstract class StateBase
{
    public virtual void OnEnter(StatePatternBase model, StateBase state) { }

    public virtual void OnUpdate(StatePatternBase model) { }

    public virtual void OnExit(StatePatternBase model, StateBase nextState) { }
}

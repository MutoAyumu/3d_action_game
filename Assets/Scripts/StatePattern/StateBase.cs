using UnityEngine;

/// <summary>
/// �eState�̃x�[�X�N���X
/// </summary>
public abstract class StateBase
{
    public virtual void OnSetup(PlayerController player) { }

    public virtual void OnEnter(PlayerController player, StateBase state) { }

    public virtual void OnUpdate(PlayerController player) { }

    public virtual void OnExit(PlayerController player, StateBase nextState) { }
}

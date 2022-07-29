using UnityEngine;

/// <summary>
/// StatePattern�̊��N���X
/// </summary>
public class StatePatternBase : MonoBehaviour
{
    protected StateBase _currentState;

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

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }

    /// <summary>
    /// State�̐؂�ւ�������
    /// </summary>
    /// <param name="nextState"></param>
    public void ChangeState(StateBase nextState)
    {
        _currentState.OnExit(this, nextState);
        nextState.OnEnter(this, _currentState);
        _currentState = nextState;
    }
}

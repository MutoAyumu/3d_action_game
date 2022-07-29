using UnityEngine;

/// <summary>
/// StatePattern‚ÌŠî’êƒNƒ‰ƒX
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

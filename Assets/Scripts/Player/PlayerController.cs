using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : StatePatternBase
{
    static readonly PlayerMoveState _moveState = new PlayerMoveState();

    [SerializeField] float _moveSpeed = 3f;

    Rigidbody _rb;
    Rigidbody Rigidbody => _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _currentState = _moveState;
    }
    protected override void OnStart()
    {
        
    }
}

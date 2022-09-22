using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    [Header("=== JumpState ===")]
    [SerializeField] float _jumpPower = 5f;
    [SerializeField] float _changeStateTime = 0.5f;

    public class PlayerJumpState : StateBase
    {
        float _changeStateTimer;
        PlayerController _player;

        public override void OnEnter(StatePatternBase entity, StateBase state)
        {
            if(!_player)
            {
                _player = entity.GetComponent<PlayerController>();
            }

            Jump();
            _changeStateTimer = 0;
        }
        public override void OnUpdate(StatePatternBase entity)
        {
            _changeStateTimer += Time.deltaTime;

            if (_player.IsGround && _changeStateTimer >= _player._changeStateTime)
            {
                _player.ChangeState(_moveState);
            }
        }
        public override void OnExit(StatePatternBase entity, StateBase nextState)
        {
            
        }

        void Jump()
        {
            _player._rb.AddForce(Vector3.up * _player._jumpPower, ForceMode.VelocityChange);
        }
    }
}
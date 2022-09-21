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

        public override void OnEnter(PlayerController player, StateBase state)
        {
            Jump(player);
            _changeStateTimer = 0;
        }
        public override void OnUpdate(PlayerController player)
        {
            _changeStateTimer += Time.deltaTime;

            if (player.IsGround && _changeStateTimer >= player._changeStateTime)
            {
                player.ChangeState(_moveState);
            }
        }
        public override void OnExit(PlayerController player, StateBase nextState)
        {
            
        }

        void Jump(PlayerController player)
        {
            player._rb.AddForce(Vector3.up * player._jumpPower, ForceMode.VelocityChange);
        }
    }
}
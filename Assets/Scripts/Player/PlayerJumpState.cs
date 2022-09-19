using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    [Header("=== JumpState ===")]
    [SerializeField] float _jumpPower = 5f;

    public class PlayerJumpState : StateBase
    {
        public override void OnEnter(PlayerController player, StateBase state)
        {
            Jump(player);
        }
        public override void OnUpdate(PlayerController player)
        {
            if (player.IsGround)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    /// <summary>
    /// �ړ��n��State�N���X
    /// </summary>
    public class PlayerMoveState : StateBase
    {
        [Header("=== MoveState ===")]
        Rigidbody _rb;�@//�L���b�V���p

        public override void OnSetup(PlayerController player)
        {
            _rb = player._rb;
        }

        public override void OnEnter(PlayerController player, StateBase state) 
        {
            
        }

        public override void OnUpdate(PlayerController player)
        {
            Move();
        }

        public override void OnExit(PlayerController player, StateBase nextState)
        { 
        
        }
        void Move()
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            var dir = new Vector3(h, _rb.velocity.y, v);
            dir.Normalize();
            _rb.velocity = dir;
        }
    }
}

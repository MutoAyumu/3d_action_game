using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    /// <summary>
    /// ˆÚ“®Œn‚ÌStateƒNƒ‰ƒX
    /// </summary>
    public class PlayerMoveState : StateBase
    {
        public override void OnEnter(StatePatternBase model, StateBase state) 
        {
            
        }

        public override void OnUpdate(StatePatternBase model)
        {
            model.Move();
        }

        public override void OnExit(StatePatternBase model, StateBase nextState)
        { 
        
        }
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

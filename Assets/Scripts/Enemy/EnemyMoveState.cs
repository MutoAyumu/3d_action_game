using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class EnemyController
{
    [Header("=== MoveState ===")]
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _stopDistance = 3f;

    public class EnemyMoveState : StateBase
    {
        EnemyController _enemy;

        public override void OnInit(StatePatternBase entity)
        {
            _enemy = entity.GetComponent<EnemyController>();
        }
        public override void OnEnter(StatePatternBase entity, StateBase state)
        {
            
        }
        public override void OnUpdate(StatePatternBase entity)
        {
            Move();
        }
        void Move()
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class EnemyBase
{
    [Header("=== MoveState ===")]
    [SerializeField] protected float _moveSpeed = 2f;

    public class EnemyMoveState : StateBase
    {
        EnemyBase _enemy;

        Rigidbody _rb;
        Transform _thisTransform;
        float _stopDistance;

        public override void OnEnter(StatePatternBase entity, StateBase state)
        {

        }
        public override void OnUpdate(StatePatternBase entity)
        {
            if (_enemy._target != null)
            {
                if(Vector3.Distance(_thisTransform.position, _enemy._target.position) < _stopDistance)
                {
                    return;
                }

                //Move();
            }
        }
        public override void OnExit(StatePatternBase entity, StateBase nextState)
        {

        }
        public override void OnInit(StatePatternBase entity)
        {
            _enemy = entity.GetComponent<EnemyBase>();

            _rb = _enemy._rb;
            _thisTransform = _enemy._thisTransform;
            _stopDistance = _enemy._stopDistance;
        }

        void Move()
        {
            var dir = _enemy._target.position - _thisTransform.position;

            var t = _enemy._target.position;

            _thisTransform.LookAt(t);

            dir.Normalize();
            dir *= _enemy._moveSpeed;

            dir.y = _enemy._rb.velocity.y;

            _enemy._rb.velocity = dir;

            Debug.Log("a");
        }
    }
}
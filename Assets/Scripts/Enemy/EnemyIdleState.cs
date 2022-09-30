using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class EnemyController
{
    [Header("=== IdleState ===")]
    [SerializeField, Tooltip("移動するまでの時間")] float _moveUpToTime = 10f;

    public class EnemyIdleState : StateBase
    {
        EnemyController _enemy;
        float _changeTime;

        public override void OnInit(StatePatternBase entity)
        {
            _enemy = entity.GetComponent<EnemyController>();
        }

        public override void OnEnter(StatePatternBase entity, StateBase state)
        {
            _changeTime = 0;
        }

        public override void OnUpdate(StatePatternBase entity)
        {
            if (!_enemy._targetTransform) return;

            _changeTime += Time.deltaTime;

            if(_changeTime >= _enemy._moveUpToTime)
            {
                _enemy.ChangeState(_enemy._moveState);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class EnemyController
{
    [Header("=== MoveState ===")]
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _stopDistance = 1f;
    [Space(10)]
    [SerializeField] Vector3 _randomPosition = new Vector3(3, 0, 3);

    public class EnemyMoveState : StateBase
    {
        EnemyController _enemy;

        //�ړ���̍��W
        Transform _destinationPoint;

        public override void OnInit(StatePatternBase entity)
        {
            _enemy = entity.GetComponent<EnemyController>();
        }
        public override void OnEnter(StatePatternBase entity, StateBase state)
        {
            var distance = Vector3.Distance(_enemy._thisTransform.position, _enemy._targetTransform.position);

            //���ȏ㋗��������Ă�����߂Â�
            if(distance > _enemy._stopDistance)
            {
                //�v���C���[�̈ʒu��ݒ�
                _destinationPoint = _enemy._targetTransform;
            }
            else
            {
                //���g�̎��̓����_���ȍ��W��ݒ�
                var randomX = _enemy._randomPosition.x;
                var randomZ = _enemy._randomPosition.z;
                _destinationPoint.position = _enemy._thisTransform.position + new Vector3(Random.Range(-randomX, randomX), _enemy._thisTransform.position.y, Random.Range(-randomZ, randomZ));
            }
        }
        public override void OnUpdate(StatePatternBase entity)
        {
            Move(_destinationPoint);
        }

        /// <summary>
        /// �w����W�܂ňړ�����
        /// </summary>
        /// <param name="position"></param>
        void Move(Transform transform)
        {
            var dir = _destinationPoint.position - _enemy._thisTransform.position;

            _enemy._rb.velocity = dir.normalized * _enemy._moveSpeed;

            var distance = Vector3.Distance(_enemy._thisTransform.position, _enemy._targetTransform.position);

            if(distance <= _enemy._stopDistance)
            {
                _enemy.ChangeState(_enemy._idleState);
            }
        }
    }
}
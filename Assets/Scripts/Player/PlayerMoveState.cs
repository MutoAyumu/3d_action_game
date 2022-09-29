using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    [Header("=== MoveState ===")]
    [SerializeField, Tooltip("�ړ��X�s�[�h")] float _moveSpeed = 5f;
    [SerializeField, Tooltip("��]�̊��炩��")] float _rotationSpeed = 5f;

    [Space(5)]

    [SerializeField] LayerMask _wallLayer;
    [SerializeField] float _wallRayLength = 2f;
    [SerializeField] Transform _chestPosition;

    [SerializeField] Transform _leftHandPosition;
    [SerializeField] Transform _rightHandPosition;

    public Transform Center => _chestPosition;

    /// <summary>
    /// �ړ��n��State�N���X
    /// </summary>
    public class PlayerMoveState : StateBase
    {
        PlayerController _player;

        public override void OnEnter(StatePatternBase entity, StateBase state) 
        {
            if(!_player)
            {
                _player = entity.GetComponent<PlayerController>();
            }
        }

        public override void OnUpdate(StatePatternBase entity)
        {
            Move();

            if (Input.GetButtonDown("Jump"))
            {
                entity.ChangeState(_jumpState);
            }
        }

        public override void OnExit(StatePatternBase entity, StateBase nextState)
        { 
        
        }
        void Move()
        {
            //����
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            //�ړ��x�N�g�����쐬
            var dir = new Vector3(h, 0, v);

            if(dir != Vector3.zero) //�ړ����͂��[������Ȃ����͈ړ��x�N�g�������Ɍ�������
            {
                dir = Camera.main.transform.TransformDirection(dir);    // �J�����̃��[�J�����W�ɕϊ�����
                dir.y = 0;  // y �������̓[���ɂ��Đ��������̃x�N�g���ɂ���

                // ���͕����Ɋ��炩�ɉ�]������
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, Time.deltaTime * _player._rotationSpeed);
            }
            //else //�J�����̑O�����Ɍ�������
            //{
            //    var vec = Camera.main.transform.forward;
            //    vec.y = 0;

            //    Quaternion targetRotation = Quaternion.LookRotation(vec);
            //    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * player._rotationSpeed);
            //}

            //var vec = Camera.main.transform.forward;
            //vec.y = 0;

            //Quaternion targetRotation = Quaternion.LookRotation(vec);
            //player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * player._rotationSpeed);

            dir.Normalize();

            _player._anim.SetFloat("IsValue", dir.magnitude, 0.2f, Time.deltaTime);

            dir *= _player._moveSpeed;
            dir.y = _player._rb.velocity.y;
            _player._rb.velocity = dir;
        }

        void WallRun(PlayerController player)
        {
            var origin = player._chestPosition ? player._chestPosition.position : player.transform.position;

            //���E�Ƀ��C�L���X�g���΂��߂�l���擾
            var right = CheckRayCast(player ,origin, player.transform.right);
            var left = CheckRayCast(player ,origin, -player.transform.right);

            if(right.collider)
            {
                player._handIK.IsRight = true;
                player._rightHandPosition.position = right.point;
                player._handIK.RightTarget = player._rightHandPosition;
            }
            else
            {
                player._handIK.IsRight = false;
            }
            
            if(left.collider)
            {
                player._handIK.IsLeft = true;
                player._leftHandPosition.position = left.point;
                player._handIK.LeftTarget = player._leftHandPosition;
            }
            else
            {
                player._handIK.IsLeft = false;
            }
        }

        /// <summary>
        /// ���C�L���X�g�̏���Ԃ��֐�
        /// </summary>
        /// <param name="player"></param>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        RaycastHit CheckRayCast(PlayerController player, Vector3 origin, Vector3 direction)
        {
            RaycastHit hit;

            Physics.Raycast(origin, direction, out hit, player._wallRayLength, player._wallLayer);

            return hit;
        }
    }
}

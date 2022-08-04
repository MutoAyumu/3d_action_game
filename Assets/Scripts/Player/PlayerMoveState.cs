using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    [Header("=== MoveState ===")]
    [SerializeField, Tooltip("�ړ��X�s�[�h")] float _moveSpeed = 5f;
    [SerializeField, Tooltip("��]�̊��炩��")] float _rotationSpeed = 5f;

    /// <summary>
    /// �ړ��n��State�N���X
    /// </summary>
    public class PlayerMoveState : StateBase
    {
        public override void OnEnter(PlayerController player, StateBase state) 
        {
            
        }

        public override void OnUpdate(PlayerController player)
        {
            Move(player);
        }

        public override void OnExit(PlayerController player, StateBase nextState)
        { 
        
        }
        void Move(PlayerController player)
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

                //// ���͕����Ɋ��炩�ɉ�]������
                //Quaternion targetRotation = Quaternion.LookRotation(dir);
                //player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * player._rotationSpeed);
            }
            //else //�J�����̑O�����Ɍ�������
            //{
            //    var vec = Camera.main.transform.forward;
            //    vec.y = 0;

            //    Quaternion targetRotation = Quaternion.LookRotation(vec);
            //    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * player._rotationSpeed);
            //}

            var vec = Camera.main.transform.forward;
            vec.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(vec);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * player._rotationSpeed);

            dir.Normalize();
            dir *= player._moveSpeed;
            dir.y = player._rb.velocity.y;
            player._rb.velocity = dir;
        }
    }
}

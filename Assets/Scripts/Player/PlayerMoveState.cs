using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    [Header("=== MoveState ===")]
    [SerializeField, Tooltip("移動スピード")] float _moveSpeed = 5f;
    [SerializeField, Tooltip("回転の滑らかさ")] float _rotationSpeed = 5f;

    /// <summary>
    /// 移動系のStateクラス
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
            //入力
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            //移動ベクトルを作成
            var dir = new Vector3(h, 0, v);

            if(dir != Vector3.zero) //移動入力がゼロじゃない時は移動ベクトル方向に向かせる
            {
                dir = Camera.main.transform.TransformDirection(dir);    // カメラのローカル座標に変換する
                dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

                //// 入力方向に滑らかに回転させる
                //Quaternion targetRotation = Quaternion.LookRotation(dir);
                //player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * player._rotationSpeed);
            }
            //else //カメラの前方向に向かせる
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

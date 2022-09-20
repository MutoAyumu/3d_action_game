using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    [Header("=== MoveState ===")]
    [SerializeField, Tooltip("移動スピード")] float _moveSpeed = 5f;
    [SerializeField, Tooltip("回転の滑らかさ")] float _rotationSpeed = 5f;

    [Space(5)]

    [SerializeField] LayerMask _wallLayer;
    [SerializeField] float _wallRayLength = 2f;
    [SerializeField] Transform _chestPosition;

    [SerializeField] Transform _leftHandPosition;
    [SerializeField] Transform _rightHandPosition;
    HandIK _handIK;

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
            //WallRun(player);

            if (Input.GetButtonDown("Jump"))
            {
                player.ChangeState(_jumpState);
            }
            if(Input.GetButtonDown("Fire1"))
            {
                player.ChangeState(_grapplingState);
            }
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

                // 入力方向に滑らかに回転させる
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * player._rotationSpeed);
            }
            //else //カメラの前方向に向かせる
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

            player._anim.SetFloat("IsValue", dir.magnitude, 0.2f, Time.deltaTime);

            dir *= player._moveSpeed;
            dir.y = player._rb.velocity.y;
            player._rb.velocity = dir;
        }

        void WallRun(PlayerController player)
        {
            var origin = player._chestPosition ? player._chestPosition.position : player.transform.position;

            //左右にレイキャストを飛ばし戻り値を取得
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
        /// レイキャストの情報を返す関数
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

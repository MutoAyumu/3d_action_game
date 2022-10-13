using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateBase = StatePatternBase<PlayerController>.StateBase;

public partial class PlayerController : MonoBehaviour
{
    #region 設置判定関係の変数
    [Header("=== 設置判定 ===")]
    [SerializeField] Transform _footPosition;
    [SerializeField] float _footPositionRadius = 0.5f;
    [SerializeField] LayerMask _footObjectLayer;
    bool _isGround;

    /// <summary>
    /// 設置判定用プロパティ
    /// </summary>
    bool IsGround => _isGround;
    #endregion

    #region 射撃関係の変数
    [Header("=== 射撃関係 ===")]
    [SerializeField] int _weaponID = 0;
    [SerializeField] WeaponType _weaponType;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] RectTransform _targetImage;
    [Space(5)]
    [SerializeField] SpriteRenderer _stencilMask;
    
    BulletRendering _bulletLine;
    WeaponModelData _weapon;
    EnemyController _targetEnemy;
    float _shotTimer;
    LookType _lookType;
    bool _isShooting;
    #endregion

    #region ステート関係の変数
    [Header("=== JumpState ===")]
    [SerializeField] float _jumpPower = 5f;
    [SerializeField] float _changeStateTime = 0.5f;

    [Header("=== MoveState ===")]
    [SerializeField, Tooltip("通常移動スピード")] float _moveSpeed = 5f;
    [SerializeField, Tooltip("覗き込み移動スピード")] float _peekMoveSpeed = 2f;
    [SerializeField, Tooltip("回転の滑らかさ")] float _rotationSpeed = 5f;
    #endregion

    [Space(5)]

    [SerializeField] LayerMask _wallLayer;
    [SerializeField] float _wallRayLength = 2f;
    [SerializeField] Transform _chestPosition;

    [SerializeField] Transform _leftHandPosition;
    [SerializeField] Transform _rightHandPosition;

    public Transform Center => _chestPosition;

    //IK関係
    HandIK _handIK;
    LookAtIK _lookAtIK;

    StatePatternBase<PlayerController> _statePattern;

    Rigidbody _rb;
    Animator _anim;
    Transform _thisTransform;

    #region 基本的なイベント関数
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _handIK = GetComponent<HandIK>();
        _lookAtIK = GetComponent<LookAtIK>();
        _bulletLine = GetComponent<BulletRendering>();
        _thisTransform = _chestPosition ? _chestPosition : this.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        _bulletLine.enabled = false;

        _statePattern = new StatePatternBase<PlayerController>(this);

        _statePattern.Add<PlayerMoveState>((int)StateType.Move);
        _statePattern.Add<PlayerJumpState>((int)StateType.Jump);

        _statePattern.OnStart((int)StateType.Move);
    }
    void Update()
    {
        _isGround = CheckGround();

        Shot();
        LookForward();
        SetTarget();

        _statePattern.OnUpdate();
    }
    #endregion

    #region 設置判定関係の関数
    /// <summary>
    /// 設置判定
    /// </summary>
    /// <returns></returns>
    bool CheckGround()
    {
        //足元の位置が設定されていなければ自信の座標を設定する
        var pos = _footPosition ? _footPosition.position : this.transform.position;
        //足元に判定用の球体を用意
        var hit = Physics.OverlapSphere(pos, _footPositionRadius, _footObjectLayer);
        bool check = false;

        if (hit.Length > 0)
        {
            check = true;
        }

        //アニメーションのBoolを設定
        _anim.SetBool("IsGround", check);

        return check;
    }
    #endregion

    #region 射撃関係の関数
    /// <summary>
    /// 射撃関数
    /// </summary>
    void Shot()
    {
        if (Input.GetButton("Fire1"))
        {
            //武器データがNullだったらreturn
            if (_weapon == null) return;

            _shotTimer += Time.deltaTime;

            //タイマーが武器の射撃速度以上になったら
            if (_shotTimer >= _weapon.ShotSpeed)
            {
                _shotTimer = 0;

                //ターゲットがNullだったらreturn
                if (!_targetEnemy) return;

                var range = _weapon.Range;
                var target = _targetEnemy.Center.position;

                //ターゲットを起点にランダムな座標を作る
                target += new Vector3(Random.Range(-range, range), Random.Range(-range, range));

                //RayCastで使う方向ベクトルを作る
                var dir = target - _thisTransform.position;

                //Hitしたオブジェクトの情報を保持しておく
                RaycastHit obj;

                var hit = Physics.Raycast(_thisTransform.position, dir, out obj, _weapon.MaxLength, _targetLayer);

                if (hit)
                {
                    //当たった対象にダメージを与える
                    var damageObj = obj.collider.GetComponent<IDamage>();
                    damageObj.Damage(_weapon.Power);
                }

                //Hitに応じてLineRendererの座標を変える
                var point = hit ? obj.point : dir + _thisTransform.position;
                _bulletLine.SetActive(true);
                //LineRendererの座標を設定
                _bulletLine.BallisticRendering(point);
                Debug.DrawRay(_thisTransform.position, dir.normalized * _weapon.MaxLength, Color.green, 0.1f);

                //return;
            }

            _bulletLine.SetActive(false);
        }
    }

    /// <summary>
    /// 覗き込む
    /// </summary>
    void LookForward()
    {
        //右クリックで覗き込む
        if (Input.GetButton("Fire2"))
        {
            if (_lookType == LookType.Peek) return;

            //タイプの変更
            _lookType = LookType.Peek;

            //カメラを切り替える
            PlayerManager.Instance.ChangeCamera(VcamType.ParsonCamera);

            //アニメーションのパラメータを変更
            _anim.SetBool("IsLookOn", true);
        }
        else
        {
            if (_lookType == LookType.Follow) return;

            _lookType = LookType.Follow;

            PlayerManager.Instance.ChangeCamera(VcamType.FollowCamera);

            _anim.SetBool("IsLookOn", false);
        }
    }

    /// <summary>
    /// 武器データを設定する
    /// </summary>
    /// <param name="data"></param>
    public void SetWeapon(WeaponModelData data)
    {
        _weapon = data;
    }

    /// <summary>
    /// ターゲット(敵)を設定
    /// </summary>
    void SetTarget()
    {
        var target = PlayerManager.Instance.Target;

        if (target)
        {
            if (target != _targetEnemy)
            {
                _targetEnemy = target;
                _lookAtIK.Target = _targetEnemy.Center;
            }
        }
        else
        {
            _lookAtIK.Target = null;
            _targetEnemy = null;
        }
    }
    #endregion

    #region Gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var pos = _footPosition ? _footPosition.position : this.transform.position;
        Gizmos.DrawWireSphere(pos, _footPositionRadius);

        var chest = _chestPosition ? _chestPosition.position : this.transform.position;
        Gizmos.DrawRay(chest, transform.right * _wallRayLength);
        Gizmos.DrawRay(chest, -transform.right * _wallRayLength);
        //Gizmos.DrawRay(, -transform.right * _wallRayLength);
    }
    #endregion

    #region Enum
    enum StateType
    {
        Move = 0,
        Jump = 1,
    }

    enum LookType
    {
        //通常
        Follow = 0,
        //覗き込み
        Peek = 1,
    }
    #endregion

    #region ステート関係

    //ステートの定義

    #region ジャンプステート
    public class PlayerJumpState : StateBase
    {
        public override void OnEnter()
        {
            Jump();
        }
        public override void OnUpdate()
        {
            StatePattern.ChangeState((int)StateType.Move);
        }

        void Jump()
        {
            Owner._rb.AddForce(Vector3.up * Owner._jumpPower, ForceMode.VelocityChange);
        }
    }
    #endregion

    #region 移動ステート
    public class PlayerMoveState : StateBase
    {
        public override void OnUpdate()
        {
            Move();

            if (Input.GetButtonDown("Jump"))
            {
                StatePattern.ChangeState((int)StateType.Jump);
            }
        }
        void Move()
        {
            //入力を取得
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            //移動ベクトルを作成
            var dir = new Vector3(h, 0, v);

            if (dir != Vector3.zero) //移動入力がゼロじゃない時は移動ベクトル方向に向かせる
            {
                dir = Camera.main.transform.TransformDirection(dir);    // カメラのローカル座標に変換する
                dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

                if (Owner._lookType == LookType.Follow)
                {
                    Rotate(dir);
                }
                else
                {
                    var vec = Camera.main.transform.forward;
                    vec.y = 0;

                    Rotate(vec);
                }
            }
            else
            {
                if (Owner._lookType == LookType.Peek)
                {
                    var vec = Camera.main.transform.forward;
                    vec.y = 0;

                    Rotate(vec);
                }
            }

            //正規化
            dir.Normalize();

            //アニメーションを設定
            Owner._anim.SetFloat("IsValue", dir.magnitude, 0.2f, Time.deltaTime);
            Owner._anim.SetFloat("Horizontal", h, 0.2f, Time.deltaTime);
            Owner._anim.SetFloat("Vertical", v, 0.2f, Time.deltaTime);

            //移動ベクトルを設定
            var speed = Owner._lookType == LookType.Follow? Owner._moveSpeed : Owner._peekMoveSpeed;
            dir *= speed;
            dir.y = Owner._rb.velocity.y;
            Owner._rb.velocity = dir;
        }
        void Rotate(Vector3 dir)
        {
            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            Owner.transform.rotation = Quaternion.Lerp(Owner.transform.rotation, targetRotation, Time.deltaTime * Owner._rotationSpeed);
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
        #endregion
    }
    #endregion
}

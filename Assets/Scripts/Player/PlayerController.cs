using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateBase = StatePatternBase<PlayerController>.StateBase;

public partial class PlayerController : MonoBehaviour
{
    [Header("=== 設置判定 ===")]
    [SerializeField] Transform _footPosition;
    [SerializeField] float _footPositionRadius = 0.5f;
    [SerializeField] LayerMask _footObjectLayer;
    bool _isGround;

    [Header("=== 射撃関係 ===")]
    [SerializeField] int _weaponID = 0;
    [SerializeField] WeaponType _weaponType;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] RectTransform _targetImage;
    BulletRendering _bulletLine;
    WeaponModelData _weapon;
    EnemyController _targetEnemy;
    float _shotTimer;
    bool _isShooting;

    [Header("=== JumpState ===")]
    [SerializeField] float _jumpPower = 5f;
    [SerializeField] float _changeStateTime = 0.5f;

    [Header("=== MoveState ===")]
    [SerializeField, Tooltip("移動スピード")] float _moveSpeed = 5f;
    [SerializeField, Tooltip("回転の滑らかさ")] float _rotationSpeed = 5f;

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

    /// <summary>
    /// 設置判定用プロパティ
    /// </summary>
    bool IsGround => _isGround;

    Rigidbody _rb;
    Animator _anim;
    Transform _thisTransform;

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
        _weapon = PlayerManager.Instance.CurrentWeapon;
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
        SetTarget();

        _statePattern.OnUpdate();
    }
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
    /// <summary>
    /// 射撃関数
    /// </summary>
    void Shot()
    {
        if (Input.GetButton("Fire2"))
        {
            _shotTimer += Time.deltaTime;
            _isShooting = true;

            PlayerManager.Instance.ChangeCamera(VcamType.ParsonCamera);

            _anim.SetBool("IsLookOn", true);
        }
        else
        {
            _isShooting = false;
            PlayerManager.Instance.ChangeCamera(VcamType.FollowCamera);

            _anim.SetBool("IsLookOn", false);
        }

        _weapon = PlayerManager.Instance.CurrentWeapon;

        if (_weapon == null) return;

        if (_shotTimer >= _weapon.ShotSpeed)
        {
            _shotTimer = 0;

            var range = _weapon.Range;

            if (!PlayerManager.Instance.Target) return;

            var target = _targetEnemy.Center.position;
            target += Camera.main.transform.TransformDirection(new Vector2(Random.Range(-range, range), Random.Range(-range, range)));

            var dir = target - _thisTransform.position;

            RaycastHit obj;

            var hit = Physics.Raycast(_thisTransform.position, dir, out obj, _weapon.MaxLength, _targetLayer);

            if (hit)
            {
                var damageObj = obj.collider.GetComponent<IDamage>();
                damageObj.Damage(_weapon.Power);
            }

            var point = hit ? obj.point : dir + _thisTransform.position;
            _bulletLine.SetActive(true);
            _bulletLine.BallisticRendering(point);
            Debug.DrawRay(_thisTransform.position, dir.normalized * _weapon.MaxLength, Color.green, 0.1f);

            return;
        }

        _bulletLine.SetActive(false);
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

    enum StateType
    {
        Move = 0,
        Jump = 1,
    }

    //ステートの定義

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

                if (!Owner._isShooting)
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
                if (Owner._isShooting)
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
            dir *= Owner._moveSpeed;
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
    }
}

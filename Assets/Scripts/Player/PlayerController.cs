using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : StatePatternBase
{
    [Header("=== 設置判定 ===")]
    [SerializeField] Transform _footPosition;
    [SerializeField] float _footPositionRadius = 0.5f;
    [SerializeField] LayerMask _footObjectLayer;
    bool _isGround;

    [Header("=== 射撃関係 ===")]
    [SerializeField] string _weaponName = "";
    [SerializeField] WeaponType _weaponType;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] RectTransform _targetImage;
    WeaponModelData _weapon;
    float _shotTimer;

    //IK関係
    HandIK _handIK;
    LookAtIK _lookAtIK;

    //各ステートを生成
    static readonly PlayerMoveState _moveState = new PlayerMoveState();
    static readonly PlayerJumpState _jumpState = new PlayerJumpState();
    static readonly PlayerGrapplingState _grapplingState = new PlayerGrapplingState();

    /// <summary>
    /// 設置判定用プロパティ
    /// </summary>
    bool IsGround => _isGround;

    Rigidbody _rb;
    Animator _anim;
    Transform _thisTransform;

    protected override void OnAwake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _handIK = GetComponent<HandIK>();
        _lookAtIK = GetComponent<LookAtIK>();
        _thisTransform = _chestPosition ? _chestPosition : this.transform;

        _currentState = _moveState;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void OnStart() 
    {
        PlayerManager.Instance.CreateData(_weaponName, _weaponType);
        _weapon = PlayerManager.Instance.GetModelData(_weaponName, _weaponType);
    }
    protected override void OnUpdate() 
    {
        _isGround = CheckGround();

        Shot();
        TargetLookAt();
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

        if(hit.Length > 0)
        {
            check = true;
        }

        //アニメーションのBoolを設定
        _anim.SetBool("IsGround", check);

        return check;
    }

    void Shot()
    {
        if(Input.GetButton("Fire1"))
        {
            _shotTimer += Time.deltaTime;
        }

        if(_shotTimer >= _weapon.ShotSpeed)
        {
            _shotTimer = 0;

            var range = _weapon.Range;

            if (!PlayerManager.Instance.Target) return;

            var target = PlayerManager.Instance.Target.Center.position;
            target += Camera.main.transform.TransformDirection(new Vector2(Random.Range(-range, range), Random.Range(-range, range)));

            var dir = target - _thisTransform.position;

            RaycastHit obj;

            var hit = Physics.Raycast(_thisTransform.position, dir, out obj, _weapon.MaxLength, _targetLayer);

            if(hit)
            {
                var damageObj = obj.collider.GetComponent<IDamage>();
                damageObj.Damage(_weapon.Power);
            }

            Debug.DrawRay(_thisTransform.position, dir.normalized * _weapon.MaxLength, Color.green, 0.1f);
        }
    }

    void TargetLookAt()
    {
        var target = PlayerManager.Instance.Target;

        if (target)
        {
            _lookAtIK.Target = target.Center;
        }
        else
        {
            if(_lookAtIK.Target)
            {
                _lookAtIK.Target = null;
            }
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
}

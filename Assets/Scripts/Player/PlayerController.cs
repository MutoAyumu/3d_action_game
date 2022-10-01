using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : StatePatternBase
{
    [Header("=== �ݒu���� ===")]
    [SerializeField] Transform _footPosition;
    [SerializeField] float _footPositionRadius = 0.5f;
    [SerializeField] LayerMask _footObjectLayer;
    bool _isGround;

    [Header("=== �ˌ��֌W ===")]
    [SerializeField] int _weaponID = 0;
    [SerializeField] WeaponType _weaponType;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] RectTransform _targetImage;
    BulletRendering _bulletLine;
    WeaponModelData _weapon;
    EnemyController _targetEnemy;
    float _shotTimer;
    bool _isShooting;

    //IK�֌W
    HandIK _handIK;
    LookAtIK _lookAtIK;

    //�e�X�e�[�g�𐶐�
    static readonly PlayerMoveState _moveState = new PlayerMoveState();
    static readonly PlayerJumpState _jumpState = new PlayerJumpState();
    static readonly PlayerGrapplingState _grapplingState = new PlayerGrapplingState();

    /// <summary>
    /// �ݒu����p�v���p�e�B
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
        _bulletLine = GetComponent<BulletRendering>();
        _thisTransform = _chestPosition ? _chestPosition : this.transform;

        _currentState = _moveState;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    protected override void OnStart()
    {
        _weapon = PlayerManager.Instance.CurrentWeapon;
    }
    protected override void OnUpdate()
    {
        _isGround = CheckGround();

        Shot();
        SetTarget();
    }
    /// <summary>
    /// �ݒu����
    /// </summary>
    /// <returns></returns>
    bool CheckGround()
    {
        //�����̈ʒu���ݒ肳��Ă��Ȃ���Ύ��M�̍��W��ݒ肷��
        var pos = _footPosition ? _footPosition.position : this.transform.position;
        //�����ɔ���p�̋��̂�p��
        var hit = Physics.OverlapSphere(pos, _footPositionRadius, _footObjectLayer);
        bool check = false;

        if (hit.Length > 0)
        {
            check = true;
        }

        //�A�j���[�V������Bool��ݒ�
        _anim.SetBool("IsGround", check);

        return check;
    }
    /// <summary>
    /// �ˌ��֐�
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
    /// �^�[�Q�b�g(�G)��ݒ�
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateBase = StatePatternBase<PlayerController>.StateBase;

public partial class PlayerController : MonoBehaviour
{
    #region �ݒu����֌W�̕ϐ�
    [Header("=== �ݒu���� ===")]
    [SerializeField] Transform _footPosition;
    [SerializeField] float _footPositionRadius = 0.5f;
    [SerializeField] LayerMask _footObjectLayer;
    bool _isGround;

    /// <summary>
    /// �ݒu����p�v���p�e�B
    /// </summary>
    bool IsGround => _isGround;
    #endregion

    #region �ˌ��֌W�̕ϐ�
    [Header("=== �ˌ��֌W ===")]
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

    #region �X�e�[�g�֌W�̕ϐ�
    [Header("=== JumpState ===")]
    [SerializeField] float _jumpPower = 5f;
    [SerializeField] float _changeStateTime = 0.5f;

    [Header("=== MoveState ===")]
    [SerializeField, Tooltip("�ʏ�ړ��X�s�[�h")] float _moveSpeed = 5f;
    [SerializeField, Tooltip("�`�����݈ړ��X�s�[�h")] float _peekMoveSpeed = 2f;
    [SerializeField, Tooltip("��]�̊��炩��")] float _rotationSpeed = 5f;
    #endregion

    [Space(5)]

    [SerializeField] LayerMask _wallLayer;
    [SerializeField] float _wallRayLength = 2f;
    [SerializeField] Transform _chestPosition;

    [SerializeField] Transform _leftHandPosition;
    [SerializeField] Transform _rightHandPosition;

    public Transform Center => _chestPosition;

    //IK�֌W
    HandIK _handIK;
    LookAtIK _lookAtIK;

    StatePatternBase<PlayerController> _statePattern;

    Rigidbody _rb;
    Animator _anim;
    Transform _thisTransform;

    #region ��{�I�ȃC�x���g�֐�
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

    #region �ݒu����֌W�̊֐�
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
    #endregion

    #region �ˌ��֌W�̊֐�
    /// <summary>
    /// �ˌ��֐�
    /// </summary>
    void Shot()
    {
        if (Input.GetButton("Fire1"))
        {
            //����f�[�^��Null��������return
            if (_weapon == null) return;

            _shotTimer += Time.deltaTime;

            //�^�C�}�[������̎ˌ����x�ȏ�ɂȂ�����
            if (_shotTimer >= _weapon.ShotSpeed)
            {
                _shotTimer = 0;

                //�^�[�Q�b�g��Null��������return
                if (!_targetEnemy) return;

                var range = _weapon.Range;
                var target = _targetEnemy.Center.position;

                //�^�[�Q�b�g���N�_�Ƀ����_���ȍ��W�����
                target += new Vector3(Random.Range(-range, range), Random.Range(-range, range));

                //RayCast�Ŏg�������x�N�g�������
                var dir = target - _thisTransform.position;

                //Hit�����I�u�W�F�N�g�̏���ێ����Ă���
                RaycastHit obj;

                var hit = Physics.Raycast(_thisTransform.position, dir, out obj, _weapon.MaxLength, _targetLayer);

                if (hit)
                {
                    //���������ΏۂɃ_���[�W��^����
                    var damageObj = obj.collider.GetComponent<IDamage>();
                    damageObj.Damage(_weapon.Power);
                }

                //Hit�ɉ�����LineRenderer�̍��W��ς���
                var point = hit ? obj.point : dir + _thisTransform.position;
                _bulletLine.SetActive(true);
                //LineRenderer�̍��W��ݒ�
                _bulletLine.BallisticRendering(point);
                Debug.DrawRay(_thisTransform.position, dir.normalized * _weapon.MaxLength, Color.green, 0.1f);

                //return;
            }

            _bulletLine.SetActive(false);
        }
    }

    /// <summary>
    /// �`������
    /// </summary>
    void LookForward()
    {
        //�E�N���b�N�Ŕ`������
        if (Input.GetButton("Fire2"))
        {
            if (_lookType == LookType.Peek) return;

            //�^�C�v�̕ύX
            _lookType = LookType.Peek;

            //�J������؂�ւ���
            PlayerManager.Instance.ChangeCamera(VcamType.ParsonCamera);

            //�A�j���[�V�����̃p�����[�^��ύX
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
    /// ����f�[�^��ݒ肷��
    /// </summary>
    /// <param name="data"></param>
    public void SetWeapon(WeaponModelData data)
    {
        _weapon = data;
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
        //�ʏ�
        Follow = 0,
        //�`������
        Peek = 1,
    }
    #endregion

    #region �X�e�[�g�֌W

    //�X�e�[�g�̒�`

    #region �W�����v�X�e�[�g
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

    #region �ړ��X�e�[�g
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
            //���͂��擾
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            //�ړ��x�N�g�����쐬
            var dir = new Vector3(h, 0, v);

            if (dir != Vector3.zero) //�ړ����͂��[������Ȃ����͈ړ��x�N�g�������Ɍ�������
            {
                dir = Camera.main.transform.TransformDirection(dir);    // �J�����̃��[�J�����W�ɕϊ�����
                dir.y = 0;  // y �������̓[���ɂ��Đ��������̃x�N�g���ɂ���

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

            //���K��
            dir.Normalize();

            //�A�j���[�V������ݒ�
            Owner._anim.SetFloat("IsValue", dir.magnitude, 0.2f, Time.deltaTime);
            Owner._anim.SetFloat("Horizontal", h, 0.2f, Time.deltaTime);
            Owner._anim.SetFloat("Vertical", v, 0.2f, Time.deltaTime);

            //�ړ��x�N�g����ݒ�
            var speed = Owner._lookType == LookType.Follow? Owner._moveSpeed : Owner._peekMoveSpeed;
            dir *= speed;
            dir.y = Owner._rb.velocity.y;
            Owner._rb.velocity = dir;
        }
        void Rotate(Vector3 dir)
        {
            // ���͕����Ɋ��炩�ɉ�]������
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            Owner.transform.rotation = Quaternion.Lerp(Owner.transform.rotation, targetRotation, Time.deltaTime * Owner._rotationSpeed);
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
        #endregion
    }
    #endregion
}

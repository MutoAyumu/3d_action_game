using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class EnemyController : StatePatternBase
{
    [Header("��{�ݒ�")]
    [SerializeField] float _radius = 3f;
    [SerializeField] float _Angle = 100f;
    [SerializeField] LayerMask _targetLayer;
    [Space(10)]
    [SerializeField] float _rotateSpeed = 5f;
    [Space(10)]
    [SerializeField, Tooltip("�������܂ł̎���")] float _missTime = 60f;
    float _missTimer;
    [Space(10)]
    [SerializeField] Transform _centerPosition;
    [Space(10)]
    [SerializeField] EnemyType _enemyType;

    Transform _thisTransform;
    Transform _targetTransform;

    Rigidbody _rb;
    Animator _anim;

    LookAtIK _lookAtIK;

    //State
    EnemyMoveState _moveState = new EnemyMoveState();
    EnemyIdleState _idleState = new EnemyIdleState();

    public Transform Center => _centerPosition;

    protected override void OnAwake()
    {
        //�L���b�V��
        _thisTransform = this.transform;

        _lookAtIK = GetComponent<LookAtIK>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        //���݂̃X�e�[�g��MoveState�ɂ���
        _currentState = _idleState;
    }
    protected override void OnStart()
    {
        _moveState.OnInit(this);
        _idleState.OnInit(this);
    }
    protected override void OnUpdate()
    {
        //�v���C���[�������Ă��Ȃ���
        if (!_targetTransform)
        {
            FindPlayer();
        }
        else
        {
            MissPlayer();
            Rotate();
        }
    }

    /// <summary>
    /// �͈͂Ńv���C���[��������
    /// </summary>
    void FindPlayer()
    {
        //���`�͈̔͂��쐬����
        var hit = Physics.OverlapSphere(_thisTransform.position, _radius, _targetLayer);

        if (hit.Length != 0)
        {
            //�擪�v�f�ŕ����x�N�g�����쐬����
            var dir = hit[0].transform.position - _thisTransform.position;

            //Vector3.Angle�Ŏ��M��Forward��dir�̊Ԃ̊p�x���擾
            var halfAngle = Vector3.Angle(_thisTransform.forward, dir);

            if (halfAngle * 2 <= _Angle)
            {
                var target = hit[0].GetComponent<PlayerController>();
                _targetTransform = target.transform;
                _lookAtIK.Target = target.Center;
            }
        }
    }

    /// <summary>
    /// �v���C���[��������
    /// </summary>
    void MissPlayer()
    {
        //�^�C�}�[���񂵂Ĉ��ȏ�ɂȂ�����Q�Ƃ�Null�ɂ���
        _missTimer += Time.deltaTime;

        if(_missTimer > _missTime)
        {
            _missTimer = 0;
            _targetTransform = null;
            _lookAtIK.Target = null;
        }
    }

    /// <summary>
    /// ��]
    /// </summary>
    void Rotate()
    {
        //�v���C���[�����Ȃ���Ζ߂�
        if (!_targetTransform) return;

        //Y���𒼂��Ă���
        var target = _targetTransform.position;
        target.y = _thisTransform.position.y;
        var rot = Quaternion.LookRotation(target - _thisTransform.position);
        _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, rot, Time.deltaTime * _rotateSpeed);
    }
    private void OnDrawGizmosSelected()
    {
        //��`��Gizmo��\��
        Handles.color = Color.red;
        Handles.DrawWireArc(this.transform.position, Vector3.up, Quaternion.Euler(0f, -_Angle / 2, 0f) * this.transform.forward, _Angle, _radius);
        Handles.DrawSolidArc(this.transform.position, Vector3.up, Quaternion.Euler(0f, -_Angle / 2, 0f) * this.transform.forward, _Angle, 1f);
    }

    enum EnemyType
    {
        NormalEnemy = 0,
        FlyEnemy = 1
    }
}

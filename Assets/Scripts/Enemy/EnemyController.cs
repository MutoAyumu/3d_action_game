using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class EnemyController : StatePatternBase
{
    [Header("基本設定")]
    [SerializeField] float _radius = 3f;
    [SerializeField] float _Angle = 100f;
    [SerializeField] LayerMask _targetLayer;
    [Space(10)]
    [SerializeField] float _rotateSpeed = 5f;
    [Space(10)]
    [SerializeField, Tooltip("見逃すまでの時間")] float _missTime = 60f;
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
        //キャッシュ
        _thisTransform = this.transform;

        _lookAtIK = GetComponent<LookAtIK>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        //現在のステートをMoveStateにする
        _currentState = _idleState;
    }
    protected override void OnStart()
    {
        _moveState.OnInit(this);
        _idleState.OnInit(this);
    }
    protected override void OnUpdate()
    {
        //プレイヤーを見つけていない時
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
    /// 範囲でプレイヤーを見つける
    /// </summary>
    void FindPlayer()
    {
        //球形の範囲を作成する
        var hit = Physics.OverlapSphere(_thisTransform.position, _radius, _targetLayer);

        if (hit.Length != 0)
        {
            //先頭要素で方向ベクトルを作成する
            var dir = hit[0].transform.position - _thisTransform.position;

            //Vector3.Angleで自信のForwardとdirの間の角度を取得
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
    /// プレイヤーを見逃す
    /// </summary>
    void MissPlayer()
    {
        //タイマーを回して一定以上になったら参照をNullにする
        _missTimer += Time.deltaTime;

        if(_missTimer > _missTime)
        {
            _missTimer = 0;
            _targetTransform = null;
            _lookAtIK.Target = null;
        }
    }

    /// <summary>
    /// 回転
    /// </summary>
    void Rotate()
    {
        //プレイヤーがいなければ戻る
        if (!_targetTransform) return;

        //Y軸を直してから
        var target = _targetTransform.position;
        target.y = _thisTransform.position.y;
        var rot = Quaternion.LookRotation(target - _thisTransform.position);
        _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, rot, Time.deltaTime * _rotateSpeed);
    }
    private void OnDrawGizmosSelected()
    {
        //扇形のGizmoを表示
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

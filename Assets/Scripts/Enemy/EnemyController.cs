using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using StateBase = StatePatternBase<EnemyController>.StateBase;

public partial class EnemyController : MonoBehaviour, IDamage
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
    [Space(10)]
    [SerializeField] int _maxHP = 1000;
    int _currentHP;

    [Header("=== MoveState ===")]
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _stopDistance = 1f;

    [Header("=== IdleState ===")]
    [SerializeField, Tooltip("移動するまでの時間")] float _moveUpToTime = 10f;

    Transform _thisTransform;
    Transform _targetTransform;

    Rigidbody _rb;
    Animator _anim;

    LookAtIK _lookAtIK;

    StatePatternBase<EnemyController> _statePattern;

    public Transform Center => _centerPosition;

    void Awake()
    {
        //キャッシュ
        _thisTransform = this.transform;
        _lookAtIK = GetComponent<LookAtIK>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentHP = _maxHP;

        _statePattern = new StatePatternBase<EnemyController>(this);

        _statePattern.Add<EnemyIdleState>((int)StateType.Idle);
        _statePattern.Add<EnemyMoveState>((int)StateType.Move);
        //_statePattern.Add<EnemyIdleState>((int)StateType.Idle);

        _statePattern.OnStart((int)StateType.Idle);
    }

    void Update()
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

        _statePattern.OnUpdate();
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

        if (_missTimer > _missTime)
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

    public void Damage(int damage)
    {
        _currentHP -= damage;

        Debug.Log($"ダメージを受けた : ダメージ {damage} : 今のHP {_currentHP}");
    }

    enum StateType
    {
        Idle = 0,
        Move = 1,
        Shot = 2,
    }

    enum EnemyType
    {
        NormalEnemy = 0,
        FlyEnemy = 1
    }

    //ステートの定義

    public class EnemyMoveState : StateBase
    {
        //移動先の座標
        Transform _destinationPoint;

        public override void OnEnter()
        {
            var distance = Vector3.Distance(Owner._thisTransform.position, Owner._targetTransform.position);

            //一定以上距離が離れていたら近づく
            if (distance > Owner._radius)
            {
                //プレイヤーの位置を設定
                _destinationPoint = Owner._targetTransform;
            }
        }
        public override void OnUpdate()
        {
            if (!_destinationPoint)
            {
                StatePattern.ChangeState((int)StateType.Idle);
            }

            Move(_destinationPoint);
        }

        public override void OnExit()
        {
            _destinationPoint = null;
        }

        /// <summary>
        /// 指定座標まで移動する
        /// </summary>
        /// <param name="position"></param>
        void Move(Transform transform)
        {
            var dir = _destinationPoint.position - Owner._thisTransform.position;
            dir.y = Owner._thisTransform.position.y;

            Owner._rb.velocity = dir.normalized * Owner._moveSpeed;

            var distance = Vector3.Distance(Owner._thisTransform.position, Owner._targetTransform.position);

            if (distance <= Owner._stopDistance)
            {
                StatePattern.ChangeState((int)StateType.Idle);
            }
        }
    }
    public class EnemyIdleState : StateBase
    {
        float _changeTime;

        public override void OnEnter()
        {
            _changeTime = 0;
        }

        public override void OnUpdate()
        {
            if (!Owner._targetTransform) return;

            _changeTime += Time.deltaTime;

            if (_changeTime >= Owner._moveUpToTime)
            {
                StatePattern.ChangeState((int)StateType.Move);
            }
        }
    }
}
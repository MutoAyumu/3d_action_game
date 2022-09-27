using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyController : StatePatternBase
{
    [Header("基本設定")]
    [SerializeField] float _radius = 3f;
    [SerializeField] LayerMask _targetLayer;
    [Space(10)]
    [SerializeField] Transform _centerPosition;
    [Space(10)]
    [SerializeField] EnemyType _enemyType;

    Transform _thisTransform;
    Transform _targetTransform;

    //State
    EnemyMoveState _moveState = new EnemyMoveState();

    public Transform Center => _centerPosition;

    protected override void OnAwake()
    {
        _thisTransform = this.transform;
        _currentState = _moveState;
    }
    protected override void OnStart()
    {
        _moveState.OnInit(this);
    }
    protected override void OnUpdate()
    {
        FindPlayer();
        Rotate();
    }

    /// <summary>
    /// 範囲でプレイヤーを見つける
    /// </summary>
    void FindPlayer()
    {
        var hit = Physics.OverlapSphere(_thisTransform.position, _radius, _targetLayer);

        if(hit.Length != 0)
        {
            _targetTransform = hit[0].transform;
            return;
        }

        _targetTransform = null;
    }

    void Rotate()
    {
        //プレイヤーがいなければ戻る
        if (!_targetTransform) return;

        _targetTransform.LookAt(_targetTransform.position);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _radius);
    }

    enum EnemyType
    {
        NormalEnemy = 0,
        FlyEnemy = 1
    }
}

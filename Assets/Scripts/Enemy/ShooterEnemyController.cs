using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ShooterEnemyController : EnemyBase
{
    static readonly EnemyMoveState _moveState = new EnemyMoveState();

    protected override void OnStart()
    {
        _moveState.OnInit(this);

        _currentState = _moveState;
    }

    protected override void OnUpdate()
    {
        _target = FindPlayer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        var center = _centerPosition ? _centerPosition.position : this.transform.position;
        center.z += _searchArea.z / 2;

        Gizmos.DrawWireCube(center, _searchArea);
    }
}

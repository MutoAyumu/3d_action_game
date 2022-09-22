using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class EnemyBase
{
    [Header("=== ShootingState ===")]
    [SerializeField] protected int _power = 1;
    [SerializeField] protected float _stopDistance = 2.5f;

    public class EnemyShootingState : StateBase
    {
        public override void OnUpdate(StatePatternBase entity)
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class EnemyBase : StatePatternBase, IDamage
{
    [Header("=== õ“GŠÖŒW ===")]
    [SerializeField] protected Vector3 _searchArea = Vector3.one;
    [SerializeField] protected Transform _centerPosition;
    [SerializeField] protected LayerMask _targetLayer;

    [Header("=== HPŠÖŒW ===")]
    [SerializeField] int _maxHP = 10000;
    [SerializeField] int _currentHP;

    protected Transform _target;

    Rigidbody _rb;
    Animator _anim;
    LineRenderer _lineRenderer;
    Transform _thisTransform;

    public Transform Center => _centerPosition ? _centerPosition : this.transform;

    protected override void OnAwake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _lineRenderer = GetComponent<LineRenderer>();
        _thisTransform = this.transform;
        _currentHP = _maxHP;
    }

    protected Transform FindPlayer()
    {
        var center = _centerPosition ? _centerPosition.position : this.transform.position;
        var half = _searchArea / 2;

        var hit = Physics.OverlapBox(center, half, Quaternion.identity, _targetLayer);

        if (hit.Length != 0)
        {
            return hit[0].transform;
        }

        return null;
    }

    public void Damage(int damage)
    {
        _currentHP -= damage;
        var eff = EffectManager._instance.InstanceDamageEffect();
        eff.Setup(_centerPosition, damage.ToString());
        Debug.Log($"{this.gameObject.name} : HP = {_currentHP} : Damage = {damage}");
    }
}

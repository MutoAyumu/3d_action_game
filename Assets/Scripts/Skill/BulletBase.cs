using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : SkillBase
{
    [Header("BulletBase")]

    [SerializeField] protected float _speed = 2f;
    [SerializeField] protected int _power = 1;
    [SerializeField] Transform _target;
    [SerializeField, Min(0)] float _time = 1;
    [SerializeField] bool _limitAcceleration = false;
    [SerializeField, Min(0)] float _maxAcceleration = 100;
    [SerializeField] float _stopDistance = 0.5f;

    Vector3 _position;
    protected Vector3 _velocity;

    float _timer;
    float _timeLimit = 2f;

    Transform _thisTransform;

    public Transform Target { get => _target; set => _target = value; }

    void Start()
    {
        //Transformをキャッシュ
        _thisTransform = transform;
        _position = _thisTransform.position;

        OnStart();
    }

    /// <summary>
    /// 派生側でStart処理を記述したいときにoverrideする
    /// </summary>
    protected virtual void OnStart() { }

    public void Update()
    {
        Shot();
    }

    void Shot()
    {
        if (_target == null)
        {
            return;
        }

        var acceleration = 2f / (_time * _time) * (_target.position - _position - _time * _velocity);

        if (_limitAcceleration && acceleration.sqrMagnitude > _maxAcceleration * _maxAcceleration)
        {
            acceleration = acceleration.normalized * _maxAcceleration;
        }

        if (_time >= 0)
        {
            _time -= Time.deltaTime;

            _velocity += acceleration * Time.deltaTime * _speed;
        }
        else
        {
            _timer += Time.deltaTime;

            if (_timer >= _timeLimit)
            {
                Destroy(this.gameObject);
            }
        }

        _position += _velocity * Time.deltaTime * _speed;
        _thisTransform.position = _position;

        _thisTransform.rotation = Quaternion.LookRotation(_velocity);

        if (Vector3.Distance(_thisTransform.position, _target.position) <= _stopDistance)
        {
            Debug.Log("当たった");
            Destroy(this.gameObject);
        }
    }
}

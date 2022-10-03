using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBulletBase : MonoBehaviour
{
    [Header("BulletBase")]

    [SerializeField] protected float _speed = 2f;
    [SerializeField] protected int _power = 1;
    [SerializeField, Min(0)] float _time = 1;
    [SerializeField] bool _limitAcceleration = false;
    [SerializeField, Min(0)] float _maxAcceleration = 100;
    [SerializeField] float _stopDistance = 0.5f;

    Vector3 _position;
    [SerializeField]Transform _target;
    protected Vector3 _velocity;

    float _timer;
    float _timeLimit = 2f;
    float _randomTime;

    TrailRenderer _trail;
    Transform _thisTransform;

    bool _isDleay = true;

    public Transform Target { get => _target; set => _target = value; }

    void Start()
    {
        _randomTime = Random.Range(0f, 1f);

        OnStart();
    }

    /// <summary>
    /// 派生側でStart処理を記述したいときにoverrideする
    /// </summary>
    protected virtual void OnStart() { }

    /// <summary>
    /// ターゲットを設定
    /// </summary>
    /// <param name="target"></param>
    public void OnSetTarget(Transform target)
    {
        _target = target;
        _trail = this.gameObject.GetComponent<TrailRenderer>();
        _trail.enabled = false;
    }

    public void Update()
    {

        if (_isDleay)
        {
            Dleay(_randomTime);
        }
        else
        {
            Shot();
        }
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

    void Dleay(float time)
    {
        _timer += Time.deltaTime;

        if(_timer >= time)
        {
            _timer = 0;
            _isDleay = false;
            this.transform.parent = null;
            _trail.enabled = true;

            //Transformをキャッシュ
            _thisTransform = transform;
            _position = _thisTransform.position;
        }
    }
}

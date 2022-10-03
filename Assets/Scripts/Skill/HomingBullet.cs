using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホーミング弾
/// </summary>
public class HomingBullet : SkillBulletBase
{
    [Header("HomingBullet")]

    [SerializeField] Vector3 _minInitVelocity;
    [SerializeField] Vector3 _maxInitVelocity = new Vector3(20,20,20);

    protected override void OnStart()
    {
        _velocity = new Vector3(Random.Range(_minInitVelocity.x, _maxInitVelocity.x), Random.Range(_minInitVelocity.y, _maxInitVelocity.y), Random.Range(_minInitVelocity.z, _maxInitVelocity.z));
    }
}

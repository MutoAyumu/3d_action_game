using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSkill : SkillBase
{
    BulletType? _type = null;
    SkillBulletBase _prefab;
    Transform _transform;

    /// <summary>
    /// コンストラクタ(番号で弾のタイプを切り替える)
    /// <code>
    /// 0 = Straight
    /// 1 = Homing</code>
    /// </summary>
    /// <param name="num">
    /// </param>
    public BulletSkill(int num, SkillBulletBase prefab, Transform transform)
    {
        switch((BulletType)num)
        {
            case BulletType.Straight:
                _type = BulletType.Straight;
                break;

            case BulletType.Homing:
                _type = BulletType.Homing;
                break;

            default:
                Debug.LogError("存在しないタイプ番号です");
                break;
        }

        _prefab = prefab;
        _transform = transform;
    }

    public override void Use()
    {
        if(_type == null)
        {
            return;
        }

        Shot();
    }

    void Shot()
    {
        for (int i = 0; i < 10; i++)
        {
            var offSet = new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), 0);

            var bullet = GameObject.Instantiate(_prefab, _transform.position + offSet, Quaternion.identity, _transform);
            bullet.OnSetTarget(PlayerManager.Instance.Target.Center.transform);
        }
    }

    enum BulletType
    {
        Straight = 0,
        Homing = 1,
    }
}
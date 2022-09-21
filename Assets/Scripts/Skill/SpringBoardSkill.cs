using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoardSkill : SkillBase
{
    SpringBoard _prefab;
    Transform _transform;
    Rigidbody _rb;
    Transform _thisTransform;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="transform"></param>
    public SpringBoardSkill(SpringBoard prefab, Transform transform, Rigidbody rigidbody)
    {
        _prefab = prefab;
        _transform = transform;
        _rb = rigidbody;
    }
    public override void Use()
    {
        var inputX = Input.GetAxisRaw("Horizontal");
        var inputY = Input.GetAxisRaw("Vertical");

        var dir = new Vector3(inputX, 0, inputY);
        dir.Normalize();

        var go = GameObject.Instantiate(_prefab, _transform.position, Quaternion.identity);

        if (dir != Vector3.zero)
        {
            dir = Camera.main.transform.TransformDirection(dir);
            dir.y = 0;
            go.OnSetup(_rb, dir + _transform.position);
            _transform.LookAt(dir + _transform.position);
        }
        else
        {
            go.OnSetup(_rb, dir);
        }
    }
}

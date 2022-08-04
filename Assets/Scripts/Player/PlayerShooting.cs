using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// プレイヤーに射撃をさせるクラス
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Vector2 _size;
    [SerializeField] float _maxDistance = 100;
    [SerializeField] LayerMask _mask;

    bool _isSearch;

    private void Update()
    {
        _isSearch = SearchArea();
    }

    bool SearchArea()
    {
        var center = this.transform.position;
        center.z += _maxDistance / 2;
        var size = new Vector3(_size.x / 2, _size.y / 2, _maxDistance / 2);

        //ここまだ
        var area = Physics.BoxCastAll(center, size, this.transform.forward, Camera.main.transform.rotation, _maxDistance, _mask);

        if (area.Length <= 0)
            return false;

        return true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = _isSearch ? Color.green : Color.red;

        Gizmos.matrix = Matrix4x4.TRS(this.transform.position, Camera.main.transform.rotation, transform.lossyScale);

        var size = new Vector3(_size.x, _size.y, _maxDistance);

        Gizmos.DrawWireCube(Vector3.forward * _maxDistance / 2, size);
    }
}

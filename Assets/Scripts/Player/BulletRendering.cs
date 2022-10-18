using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
/// <summary>
/// 弾道を描画する
/// </summary>
public class BulletRendering : MonoBehaviour
{
    [SerializeField, Tooltip("線の太さ")] float _width = 0.01f;
    [SerializeField, Tooltip("線の色")] Color _color = Color.green;
    [SerializeField] Transform _muzzle;

    LineRenderer _line;
    Vector3 _setPoint;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();

        //線の太さを設定
        _line.startWidth = _width;
        _line.endWidth = _width;

        //線の色を設定
        _line.startColor = _color;
        _line.endColor = _color;
    }

    /// <summary>
    /// 弾道を描画する
    /// </summary>
    public void BallisticRendering(Vector3 point)
    {
        _setPoint = point;

        var position = _muzzle.position;

        //線の位置配列を作成
        var points = new Vector3[]
        {
            position,
            _setPoint,
        };

        //線を引く位置を設定
        _line.SetPositions(points);
    }

    /// <summary>
    /// コンポーネントのオンオフを切り替える
    /// </summary>
    /// <param name="flag"></param>
    public void SetEnabled(bool flag)
    {
        _line.enabled = flag;
    }
}

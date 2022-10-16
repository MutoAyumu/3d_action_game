using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
/// <summary>
/// �e����`�悷��
/// </summary>
public class BulletRendering : MonoBehaviour
{
    [SerializeField, Tooltip("���̑���")] float _width = 0.01f;
    [SerializeField, Tooltip("���̐F")] Color _color = Color.green;
    [SerializeField] Transform _muzzle;

    LineRenderer _line;
    Vector3 _setPoint;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();

        //���̑�����ݒ�
        _line.startWidth = _width;
        _line.endWidth = _width;

        //���̐F��ݒ�
        _line.startColor = _color;
        _line.endColor = _color;
    }

    /// <summary>
    /// �e����`�悷��
    /// </summary>
    public void BallisticRendering(Vector3 point)
    {
        _setPoint = point;

        var position = _muzzle.position;

        //���̈ʒu�z����쐬
        var points = new Vector3[]
        {
            position,
            _setPoint,
        };

        //���������ʒu��ݒ�
        _line.SetPositions(points);
    }

    /// <summary>
    /// �R���|�[�l���g�̃I���I�t��؂�ւ���
    /// </summary>
    /// <param name="flag"></param>
    public void SetEnabled(bool flag)
    {
        _line.enabled = flag;
    }
}

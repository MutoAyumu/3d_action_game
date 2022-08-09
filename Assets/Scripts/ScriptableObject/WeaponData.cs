using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New WeaponData", fileName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Information")]
    [SerializeField, Tooltip("����̎��")] WeaponType _type;
    [SerializeField, Tooltip("���O")] string _name;
    [SerializeField, Tooltip("�U������")] float _maxLength = 10f;
    [SerializeField, Tooltip("�e���U�e����͈�")] Vector2 _randomRange = Vector2.one;
    //�����Ƀ��f�����ǉ����邩������Ȃ�

    public WeaponType Type => _type;
    public string Name => _name;
    public float MaxLength => _maxLength;
    public Vector2 RandomRange => _randomRange;
}
public enum WeaponType
{
    AR,     //�A�T���g����
    SMG,    //�T�u�}�V���K������
}

[System.Serializable]
public class WeaponModelData
{
    public WeaponType Type;
    public string Name;
    public float MaxLength;
    public float Range;
}

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
    [SerializeField, Tooltip("�e���U�e����͈�")] float _range = 1f;
    [SerializeField, Tooltip("�U����")] int _power = 10;
    [SerializeField, Tooltip("���ˑ��x")] float _shotSpeed = 0.1f;
    //�����Ƀ��f�����ǉ����邩������Ȃ�

    public WeaponType Type => _type;
    public string Name => _name;
    public float MaxLength => _maxLength;
    public float Range => _range;
    public int Power => _power;
    public float ShotSpeed => _shotSpeed;
}
public enum WeaponType
{
    AR = 0,     //�A�T���g����
    SMG = 1,    //�T�u�}�V���K������
}

[System.Serializable]
public class WeaponModelData
{
    public WeaponType Type;
    public string Name;
    public int ID;
    public float MaxLength;
    public float Range;
    public int Power;
    public float ShotSpeed;
}

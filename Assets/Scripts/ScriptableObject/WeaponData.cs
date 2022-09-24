using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New WeaponData", fileName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Information")]
    [SerializeField, Tooltip("武器の種類")] WeaponType _type;
    [SerializeField, Tooltip("名前")] string _name;
    [SerializeField, Tooltip("攻撃距離")] float _maxLength = 10f;
    [SerializeField, Tooltip("弾が散弾する範囲")] float _range = 1f;
    [SerializeField, Tooltip("攻撃力")] int _power = 10;
    [SerializeField, Tooltip("発射速度")] float _shotSpeed = 0.1f;
    //ここにモデルも追加するかもしれない

    public WeaponType Type => _type;
    public string Name => _name;
    public float MaxLength => _maxLength;
    public float Range => _range;
    public int Power => _power;
    public float ShotSpeed => _shotSpeed;
}
public enum WeaponType
{
    AR = 0,     //アサルト武器
    SMG = 1,    //サブマシンガン武器
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

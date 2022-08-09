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
    [SerializeField, Tooltip("弾が散弾する範囲")] Vector2 _randomRange = Vector2.one;
    //ここにモデルも追加するかもしれない

    public WeaponType Type => _type;
    public string Name => _name;
    public float MaxLength => _maxLength;
    public Vector2 RandomRange => _randomRange;
}
public enum WeaponType
{
    AR,     //アサルト武器
    SMG,    //サブマシンガン武器
}

[System.Serializable]
public class WeaponModelData
{
    public WeaponType Type;
    public string Name;
    public float MaxLength;
    public float Range;
}

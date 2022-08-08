using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New WeaponData", fileName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Information")]
    [SerializeField] Type _type;
    [SerializeField] string _name;

    public Type WeaponType => _type;
    public string Name => _name;

    public enum Type
    {
        AR,     //アサルト武器
        SMG,    //サブマシンガン武器
    }
}

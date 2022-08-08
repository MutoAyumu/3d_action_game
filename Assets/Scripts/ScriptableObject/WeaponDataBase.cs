using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New DataBase", fileName = "WeaponDataBase")]
public class WeaponDataBase : ScriptableObject
{
    [SerializeField] WeaponData[] _weapons;
}

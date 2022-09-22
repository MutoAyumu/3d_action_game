using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Weapon/New DataBase", fileName = "WeaponDataBase")]
public class WeaponDataBase : ScriptableObject
{
    [SerializeField] WeaponData[] _weapons;

    /// <summary>
    /// �f�[�^�̎擾
    /// </summary>
    /// <param name="n"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public WeaponModelData GetData(string n, WeaponType t)
    {
        var data = Init(n,t);

        if(!data) return null;

        var type = data.Type;
        var name = data.Name;
        var length = data.MaxLength;
        var range = data.Range;
        var power = data.Power;
        var speed = data.ShotSpeed;

        //�쐬�����f�[�^��Ԃ�
        return new WeaponModelData() { Type = type, Name = name, MaxLength = length, Range = range , Power = power, ShotSpeed = speed};
    }
    WeaponData Init(string name, WeaponType type)
    {
        //�f�[�^�̌���
        var data = _weapons.Where(x => x.Name == name && x.Type == type);
        WeaponData weapon = null;

        if(data.Count() >= 2)
        {
            weapon = data.First();
            Debug.LogError($"�����̃f�[�^��������܂����B�擪�f�[�^��Ԃ��܂� : {weapon}");
        }
        else if(data.Count() == 0)
        {
            Debug.LogError($"�w�肵�����O�E�^�C�v�̃f�[�^�����݂��܂���");
        }
        else
        {
            weapon = data.Single();
            Debug.Log($"�w��f�[�^��������܂��� : {weapon}");
        }

        return weapon;
    }
}

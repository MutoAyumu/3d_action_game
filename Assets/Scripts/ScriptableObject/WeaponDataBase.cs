using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Weapon/New DataBase", fileName = "WeaponDataBase")]
public class WeaponDataBase : ScriptableObject
{
    [SerializeField] WeaponData[] _weapons;

    int[] _weaponsID;
 
    private void OnEnable()
    {
        //ScriptableObject��OnEnable�̓Q�[��(�����^�C��)�����߂ēǂݍ��܂ꂽ����1�x��������
        //�����������炨�������Ȃ邩���i�ςȋ����������炱���𒼂��j
        _weaponsID = new int[_weapons.Length];
    }

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

        //�^�C�v�ɂ���đ���������ID��ς���
        var id = 0;

        switch(type)
        {
            case WeaponType.AR:
                id = _weaponsID[0];
                _weaponsID[0]++;
                break;

            case WeaponType.SMG:
                id = _weaponsID[1];
                _weaponsID[1]++;
                break;
        }

        //�쐬�����f�[�^��Ԃ�
        return new WeaponModelData() { Type = type, Name = name, ID = id, MaxLength = length, Range = range , Power = power, ShotSpeed = speed};
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

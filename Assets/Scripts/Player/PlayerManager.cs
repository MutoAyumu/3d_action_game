using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager
{
    static PlayerManager _instance = new PlayerManager();
    public static PlayerManager Instance => _instance;

    public EnemyBase Target { get => _target; set => _target = value; }

    List<WeaponModelData> _modelList = new List<WeaponModelData>();
    WeaponDataBase _dataBase;

    EnemyBase _target;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public PlayerManager() { }

    public void Setup(PlayerManagerAttachment attachment)
    {
        _dataBase = attachment.DataBase;
    }
    
    /// <summary>
    /// ����f�[�^�̍쐬
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public void CreateData(string name, WeaponType type)
    {
        var data = _dataBase.GetData(name, type);

        if (data != null)
        {
            Debug.Log($"{data} : [Name = {data.Name} : Type = {data.Type} : Length = {data.MaxLength} : Range = {data.Range} : Power = {data.Power} : ShotSpeed = {data.ShotSpeed}]");
            _modelList.Add(data);
        }
    }

    /// <summary>
    /// ���X�g����f�[�^������
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public WeaponModelData GetModelData(string name, WeaponType type)
    {
        var data = _modelList.Where(x => x.Name == name && x.Type == type);
        WeaponModelData weapon = null;

        if (data.Count() >= 2)
        {
            weapon = data.First();
            Debug.LogError($"�����̃f�[�^��������܂����B�擪�f�[�^��Ԃ��܂� : {weapon}");
        }
        else if (data.Count() == 0)
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

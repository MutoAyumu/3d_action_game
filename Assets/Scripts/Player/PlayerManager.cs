using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager
{
    static PlayerManager _instance = new PlayerManager();
    public static PlayerManager Instance => _instance;

    public EnemyController Target { get => _target; set => _target = value; }

    /// <summary>
    /// �������Ă��镐��̃��X�g
    /// </summary>
    public List<WeaponModelData> WeaponModels => _modelList;

    List<WeaponModelData> _modelList = new List<WeaponModelData>();
    WeaponDataBase _dataBase;

    EnemyController _target;

    WeaponModelData _currentWeapon;

    /// <summary>
    /// ���ݎg���Ă��镐��
    /// </summary>
    public WeaponModelData CurrentWeapon => _currentWeapon;

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
            Debug.Log($"{data} : [Name = {data.Name} : Type = {data.Type} : ID = {data.ID} : Length = {data.MaxLength} : Range = {data.Range} : Power = {data.Power} : ShotSpeed = {data.ShotSpeed}]");
            _modelList.Add(data);
        }
    }

    /// <summary>
    /// ���X�g����f�[�^������
    /// <code>
    /// ID��Type�Ŏ擾����
    /// </code>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public WeaponModelData GetModelData(int id, WeaponType type)
    {
        var data = _modelList.Where(x => x.ID == id && x.Type == type);
        WeaponModelData weapon = null;

        if (data.Count() == 0)
        {
            Debug.LogError($"�w�肵�����O�E�^�C�v�̃f�[�^�����݂��܂���");
        }
        else
        {
            weapon = data.Single();
            Debug.Log($"�w��f�[�^��������܂��� : Name = {weapon.Name} : ID = {weapon.ID} : Type = {weapon.Type}");
        }

        return weapon;
    }

    /// <summary>
    /// �����؂�ւ���
    /// </summary>
    /// <param name="data"></param>
    public void ChangeWeapon(WeaponModelData data)
    {
        _currentWeapon = data;
    }
}

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
    /// コンストラクタ
    /// </summary>
    public PlayerManager() { }

    public void Setup(PlayerManagerAttachment attachment)
    {
        _dataBase = attachment.DataBase;
    }
    
    /// <summary>
    /// 武器データの作成
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
    /// リストからデータを検索
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
            Debug.LogError($"複数のデータが見つかりました。先頭データを返します : {weapon}");
        }
        else if (data.Count() == 0)
        {
            Debug.LogError($"指定した名前・タイプのデータが存在しません");
        }
        else
        {
            weapon = data.Single();
            Debug.Log($"指定データが見つかりました : {weapon}");
        }

        return weapon;
    }


}

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
    /// 所持している武器のリスト
    /// </summary>
    public List<WeaponModelData> WeaponModels => _modelList;

    List<WeaponModelData> _modelList = new List<WeaponModelData>();
    WeaponDataBase _dataBase;

    EnemyController _target;

    WeaponModelData _currentWeapon;

    /// <summary>
    /// 現在使っている武器
    /// </summary>
    public WeaponModelData CurrentWeapon => _currentWeapon;

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
            Debug.Log($"{data} : [Name = {data.Name} : Type = {data.Type} : ID = {data.ID} : Length = {data.MaxLength} : Range = {data.Range} : Power = {data.Power} : ShotSpeed = {data.ShotSpeed}]");
            _modelList.Add(data);
        }
    }

    /// <summary>
    /// リストからデータを検索
    /// <code>
    /// IDとTypeで取得する
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
            Debug.LogError($"指定した名前・タイプのデータが存在しません");
        }
        else
        {
            weapon = data.Single();
            Debug.Log($"指定データが見つかりました : Name = {weapon.Name} : ID = {weapon.ID} : Type = {weapon.Type}");
        }

        return weapon;
    }

    /// <summary>
    /// 武器を切り替える
    /// </summary>
    /// <param name="data"></param>
    public void ChangeWeapon(WeaponModelData data)
    {
        _currentWeapon = data;
    }
}

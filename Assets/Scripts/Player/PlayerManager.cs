using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UniRx;

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

    CinemachineVirtualCamera[] _vcamArray;
    VcamType _currentCameraType = VcamType.FollowCamera;

    PlayerController _player;

    public VcamType CameraType => _currentCameraType;

    EnemyController _target;

    WeaponModelData _currentWeapon;
    ReactiveProperty<WeaponModelData> _currentSelectButton;

    /// <summary>
    /// 現在使っている武器
    /// </summary>
    public WeaponModelData CurrentWeapon => _currentWeapon;

    public IReadOnlyReactiveProperty<WeaponModelData> CurrentSelectButton => _currentSelectButton;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PlayerManager() { }

    public void Setup(PlayerManagerAttachment attachment)
    {
        _dataBase = attachment.DataBase;
        _vcamArray = attachment.CameraArray;
        _player = attachment.Player;

        _currentSelectButton = new ReactiveProperty<WeaponModelData>();
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
        _player.SetWeapon(_currentWeapon);
    }

    public void SelectWeaponButton(WeaponModelData data)
    {
        _currentSelectButton.Value = data;
    }

    /// <summary>
    /// カメラを切り替える
    /// </summary>
    public void ChangeCamera(VcamType type)
    {
        //タイプに応じて使用するカメラのプライオリティを切り替える
        switch(type)
        {
            case VcamType.FollowCamera:
                _vcamArray[0].MoveToTopOfPrioritySubqueue();
                break;
            case VcamType.ParsonCamera:
                _vcamArray[1].MoveToTopOfPrioritySubqueue();
                break;
        }

        _currentCameraType = type;
    }
}
public enum VcamType
{
    FollowCamera = 0,
    ParsonCamera = 1,
}

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
        //ScriptableObjectのOnEnableはゲーム(ランタイム)中初めて読み込まれた時に1度だけ走る
        //もしかしたらおかしくなるかも（変な挙動をしたらここを直す）
        _weaponsID = new int[_weapons.Length];
    }

    /// <summary>
    /// データの取得
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

        //タイプによって増加させるIDを変える
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

        //作成したデータを返す
        return new WeaponModelData() { Type = type, Name = name, ID = id, MaxLength = length, Range = range , Power = power, ShotSpeed = speed};
    }
    WeaponData Init(string name, WeaponType type)
    {
        //データの検索
        var data = _weapons.Where(x => x.Name == name && x.Type == type);
        WeaponData weapon = null;

        if(data.Count() >= 2)
        {
            weapon = data.First();
            Debug.LogError($"複数のデータが見つかりました。先頭データを返します : {weapon}");
        }
        else if(data.Count() == 0)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerAttachment : MonoBehaviour
{
    [SerializeField] WeaponDataBase _dataBase; //Resourcesから動的に読み込む方法にする可能性あり

    [Header("Debug")]
    [SerializeField] string _name;
    [SerializeField] WeaponType _type;
    public WeaponDataBase DataBase => _dataBase;

    private void Awake()
    {
        PlayerManager.Instance.Setup(this);
    }
    private void Start()
    {
        PlayerManager.Instance.CreateData(_name, _type)
;    }
}

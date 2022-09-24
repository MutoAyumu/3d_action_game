using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerAttachment : MonoBehaviour
{
    [SerializeField] WeaponDataBase _dataBase; //Resources‚©‚ç“®“I‚É“Ç‚Ýž‚Þ•û–@‚É‚·‚é‰Â”\«‚ ‚è
    [SerializeField]WeaponSelectUI _weaponSelectUI;

    [Header("Debug")]
    [SerializeField] DebugData[] _data;
    public WeaponDataBase DataBase => _dataBase;

    private void Awake()
    {
        PlayerManager.Instance.Setup(this);

    }
    private void Start()
    {
        //0”Ô–Ú‚ð”ò‚Î‚·
        for (int i = 1; i < _data.Length; i++)
        {
            PlayerManager.Instance.CreateData(_data[i].Name, _data[i].Type);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(_weaponSelectUI.IsActive)
            {
                _weaponSelectUI.CloseWindow();
            }
            else
            {
                _weaponSelectUI.OpenWindow();
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            PlayerManager.Instance.CreateData(_data[1].Name, _data[1].Type);
        }
    }
}
[System.Serializable]

public class DebugData
{
    [SerializeField] string _name;
    [SerializeField] WeaponType _type;

    public string Name => _name;
    public WeaponType Type => _type;
}

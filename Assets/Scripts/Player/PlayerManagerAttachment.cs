using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerAttachment : MonoBehaviour
{
    [SerializeField] WeaponDataBase _dataBase; //Resources‚©‚ç“®“I‚É“Ç‚Ýž‚Þ•û–@‚É‚·‚é‰Â”\«‚ ‚è

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
        //PlayerManager.Instance.CreateData(_name, _type)
;    }
}

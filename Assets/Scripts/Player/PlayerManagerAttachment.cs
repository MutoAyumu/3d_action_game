using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerAttachment : MonoBehaviour
{
    [SerializeField] WeaponDataBase _dataBase;

    public delegate void SetupMethod();
    SetupMethod _setup;

    private void Awake()
    {
        
    }

    //public void SetupCallback(SetupMethod setup)
    //{
    //    _setup = setup;
    //}
}

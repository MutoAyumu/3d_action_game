using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerAttachment : MonoBehaviour
{
    [SerializeField] WeaponDataBase _dataBase; //Resources���瓮�I�ɓǂݍ��ޕ��@�ɂ���\������
    [SerializeField] WeaponSelectUI _weaponSelectUI;

    /// <summary>
    /// �O��FollowCamera�@�P��3rdParsonCamera
    /// </summary>
    [Header("Vcam")]
    [SerializeField,Tooltip("�O��FollowCamera�@�P��3rdParsonCamera")] CinemachineVirtualCamera[] _vcam;
    CinemachinePOV _followPOV;
    CinemachinePOV _3rdParsonPOV;
    Cinemachine3rdPersonFollow a;

    public CinemachineVirtualCamera[] CameraArray => _vcam;

    [Header("Debug")]
    [SerializeField] DebugData[] _data;
    public WeaponDataBase DataBase => _dataBase;

    private void Awake()
    {
        PlayerManager.Instance.Setup(this);
        _followPOV = _vcam[0].GetCinemachineComponent(CinemachineCore.Stage.Aim).GetComponent<CinemachinePOV>();
        _3rdParsonPOV = _vcam[1].GetCinemachineComponent(CinemachineCore.Stage.Aim).GetComponent<CinemachinePOV>();
    }
    private void Start()
    {
        //0�Ԗڂ��΂�
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
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                _weaponSelectUI.OpenWindow();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            PlayerManager.Instance.CreateData(_data[1].Name, _data[1].Type);
        }

        MatchCameraSettings();
    }

    /// <summary>
    /// Vcam�Ŏg�p�����J�����̐ݒ�����킹��
    /// </summary>
    void MatchCameraSettings()
    {
        switch(PlayerManager.Instance.CameraType)
        {
            case VcamType.FollowCamera:
                _3rdParsonPOV.m_HorizontalAxis.Value = _followPOV.m_HorizontalAxis.Value;
                break;
            case VcamType.ParsonCamera:
                _followPOV.m_HorizontalAxis.Value = _3rdParsonPOV.m_HorizontalAxis.Value;
                _followPOV.m_VerticalAxis.Value = _3rdParsonPOV.m_VerticalAxis.Value;
                break;
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

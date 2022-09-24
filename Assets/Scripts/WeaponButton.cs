using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    [SerializeField] Text _text;

    int _id;
    WeaponType _type;

    private void Awake()
    {
        //OnClick�ɓo�^
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => ChangeData());
    }

    public void OnSetData(WeaponModelData data)
    {
        _id = data.ID;
        _type = data.Type;
        _text.text = data.Name;
    }

    /// <summary>
    /// ����f�[�^��؂�ւ���
    /// </summary>
    public void ChangeData()
    {
        var data = PlayerManager.Instance.GetModelData(_id, _type);
        PlayerManager.Instance.ChangeWeapon(data);
    }
}

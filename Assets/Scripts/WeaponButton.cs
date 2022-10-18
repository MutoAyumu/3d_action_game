using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField]Image _image;

    WeaponModelData _data;

    private void Awake()
    {
        //OnClickに登録
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => ChangeData());
    }

    public void OnSetData(WeaponModelData data)
    {
        _data = data;
        _text.text = data.Name;

        SetTypeIcon(_data.Type);
    }

    void SetTypeIcon(WeaponType type)
    {
        Sprite sprite = default;

        //Resourcesからアイコンを読み込む
        switch(type)
        {
            case WeaponType.AR:
                sprite = Resources.Load<Sprite>("WeaponIcon/ARIcon");
                break;
            case WeaponType.SMG:
                sprite = Resources.Load<Sprite>("WeaponIcon/SMGIcon");
                break;
        }

        _image.sprite = sprite;
    }

    /// <summary>
    /// 武器データを切り替える
    /// </summary>
    public void ChangeData()
    {
        PlayerManager.Instance.ChangeWeapon(_data);
    }

    public void ChangeSelectData()
    {
        PlayerManager.Instance.SelectWeaponButton(_data);
    }
}

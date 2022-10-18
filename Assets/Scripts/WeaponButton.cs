using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField]Image _image;

    int _id;
    WeaponType _type;

    private void Awake()
    {
        //OnClickに登録
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => ChangeData());
    }

    public void OnSetData(WeaponModelData data)
    {
        _id = data.ID;
        _type = data.Type;
        _text.text = data.Name;

        SetTypeIcon(_type);
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
        var data = PlayerManager.Instance.GetModelData(_id, _type);
        PlayerManager.Instance.ChangeWeapon(data);
    }
}

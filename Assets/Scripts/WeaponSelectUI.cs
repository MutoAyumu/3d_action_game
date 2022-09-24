using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectUI : MonoBehaviour
{
    [SerializeField] WeaponButton _buttonPrefab;
    [SerializeField] RectTransform _rectTransform;

    List<WeaponButton> _buttonList = new List<WeaponButton>();

    public bool IsActive => this.gameObject.activeSelf;

    //持っている武器の個数分ボタンを生成・配置する
    //ボタンに武器のデータを渡す
    //リストに追加する
    //持っている武器の情報が更新されたら、更新分のボタンを生成・リストに追加

    /// <summary>
    /// UIウィンドウを表示させる
    /// </summary>
    public void OpenWindow()
    {
        //ウィンドウを開くときにデータの更新を確認して、必要があれば追加する

        this.gameObject.gameObject.SetActive(true);

        //所持している武器の個数
        var data = PlayerManager.Instance.WeaponModels;
        var dataCount = data.Count;

        //リストに追加されているボタンの個数
        var listCount = _buttonList.Count;

        //新規作成するボタンの数
        var num = dataCount - listCount;

        //データが追加されていた時
        if (num > 0)
        {
            for (int i = 0; i < num; i++)
            {
                //既に追加されているボタンの続きから作成
                CreateButton(data[listCount + i]);
            }
        }
        //データが削除されていた時
        else if(num < 0)
        {

        }
    }

    public void CloseWindow()
    {
        this.gameObject.gameObject.SetActive(false);
    }

    void CreateButton(WeaponModelData data)
    {
        if (!_buttonPrefab)
        {
            Debug.LogError($"ボタンのPrefabがありません");
            return;
        }

        //ボタンを作成
        var button = Instantiate(_buttonPrefab, _buttonPrefab.transform.position, Quaternion.identity, _rectTransform);

        //データをボタン側に追加
        button.OnSetData(data);

        //リストに追加
        _buttonList.Add(button);
    }
}

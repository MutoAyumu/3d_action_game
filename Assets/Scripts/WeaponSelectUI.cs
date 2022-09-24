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

    //�����Ă��镐��̌����{�^���𐶐��E�z�u����
    //�{�^���ɕ���̃f�[�^��n��
    //���X�g�ɒǉ�����
    //�����Ă��镐��̏�񂪍X�V���ꂽ��A�X�V���̃{�^���𐶐��E���X�g�ɒǉ�

    /// <summary>
    /// UI�E�B���h�E��\��������
    /// </summary>
    public void OpenWindow()
    {
        //�E�B���h�E���J���Ƃ��Ƀf�[�^�̍X�V���m�F���āA�K�v������Βǉ�����

        this.gameObject.gameObject.SetActive(true);

        //�������Ă��镐��̌�
        var data = PlayerManager.Instance.WeaponModels;
        var dataCount = data.Count;

        //���X�g�ɒǉ�����Ă���{�^���̌�
        var listCount = _buttonList.Count;

        //�V�K�쐬����{�^���̐�
        var num = dataCount - listCount;

        //�f�[�^���ǉ�����Ă�����
        if (num > 0)
        {
            for (int i = 0; i < num; i++)
            {
                //���ɒǉ�����Ă���{�^���̑�������쐬
                CreateButton(data[listCount + i]);
            }
        }
        //�f�[�^���폜����Ă�����
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
            Debug.LogError($"�{�^����Prefab������܂���");
            return;
        }

        //�{�^�����쐬
        var button = Instantiate(_buttonPrefab, _buttonPrefab.transform.position, Quaternion.identity, _rectTransform);

        //�f�[�^���{�^�����ɒǉ�
        button.OnSetData(data);

        //���X�g�ɒǉ�
        _buttonList.Add(button);
    }
}

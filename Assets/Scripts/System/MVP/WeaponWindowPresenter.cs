using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class WeaponWindowPresenter : MonoBehaviour
{
    [Header("•Ší‚ÌUIŠÖŒW")]
    [SerializeField] MVPText _nameText;
    [SerializeField] MVPText _powerText;
    [SerializeField] MVPText _lengthText;

    void SetWeaponUI()
    {
        if(_nameText)
        {
            PlayerManager.Instance.CurrentSelectButton.Subscribe(x =>
            {
                if (x == null) return;

                _nameText.SetText(x.Name);
            }).AddTo(this);
        }

        if (_powerText)
        {
            PlayerManager.Instance.CurrentSelectButton.Subscribe(x =>
            {
                if (x == null) return;

                _powerText.SetText(x.Power.ToString());
            }).AddTo(this);
        }

        if (_lengthText)
        {
            PlayerManager.Instance.CurrentSelectButton.Subscribe(x =>
            {
                if (x == null) return;

                _lengthText.SetText(x.MaxLength.ToString());
            }).AddTo(this);
        }
    }

    private void Start()
    {
        SetWeaponUI(); 
    }
}

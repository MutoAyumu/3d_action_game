using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MVPText : MonoBehaviour
{
    Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    public void SetText(string t)
    {
        if(_text)
        {
            _text.text = t;
        }
    }
}

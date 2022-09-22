using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] AnimationCurve _animationCurve;

    Text _text;
    RectTransform _rect;
    Vector3 _mov;
    Vector3 _movVec = new Vector3(0, 1f, 0);
    Vector2 _random;
    Vector3? _target;
    float _timer;

    private void Awake()
    {
        _text = GetComponent<Text>();
        _rect = GetComponent<RectTransform>();
        _random = new Vector2(Random.Range(-50f, 50f), Random.Range(-25f, 25f));
    }

    public void Setup(Transform target, string text)
    {
        _target = target.position;
        _text.text = text;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        _mov += _movVec * _animationCurve.Evaluate(_timer);
        _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.Value) + _random;
        _rect.position += _mov;

        var c = _text.color;
        c.a -= Time.deltaTime;
        _text.color = c;

        if (_timer >= 1f)
        {
            Destroy(this.gameObject);
        }
    }
}

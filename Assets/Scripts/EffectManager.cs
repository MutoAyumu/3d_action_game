using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
    public static EffectManager _instance = new EffectManager();

    public EffectManager() { }

    DamageEffect _damageEffectPrefab;
    Canvas _canvas;

    public void OnSetup(EffectManagerattAchment achment)
    {
        _damageEffectPrefab = achment.DamageEffectPrefab;
        _canvas = achment.EnemyUI;
    }

    public DamageEffect InstanceDamageEffect()
    {
        return GameObject.Instantiate(_damageEffectPrefab, Vector3.zero, Quaternion.identity, _canvas.transform);
    }
}

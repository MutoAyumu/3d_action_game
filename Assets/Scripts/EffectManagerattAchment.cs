using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManagerattAchment : MonoBehaviour
{
    [SerializeField] DamageEffect _damageEffectPrefab;
    [SerializeField] Canvas _enemyUI;

    public DamageEffect DamageEffectPrefab => _damageEffectPrefab;
    public Canvas EnemyUI => _enemyUI;

    private void Awake()
    {
        EffectManager._instance.OnSetup(this);
    }
}

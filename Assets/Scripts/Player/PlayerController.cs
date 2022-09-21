using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [Header("=== 設置判定 ===")]
    [SerializeField] Transform _footPosition;
    [SerializeField] float _footPositionRadius = 0.5f;
    [SerializeField] LayerMask _footObjectLayer;
    bool _isGround;

    /// <summary>
    /// 設置判定用プロパティ
    /// </summary>
    bool IsGround => _isGround;

    Rigidbody _rb;
    Animator _anim;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _handIK = GetComponent<HandIK>();
    }

    void OnStart() 
    {

    }
    void OnUpdate() 
    {
        _isGround = CheckGround();
    }

    /// <summary>
    /// 設置判定
    /// </summary>
    /// <returns></returns>
    bool CheckGround()
    {
        //足元の位置が設定されていなければ自信の座標を設定する
        var pos = _footPosition ? _footPosition.position : this.transform.position;
        //足元に判定用の球体を用意
        var hit = Physics.OverlapSphere(pos, _footPositionRadius, _footObjectLayer);
        bool check = false;

        if(hit.Length > 0)
        {
            check = true;
        }

        //アニメーションのBoolを設定
        _anim.SetBool("IsGround", check);

        return check;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var pos = _footPosition ? _footPosition.position : this.transform.position;
        Gizmos.DrawWireSphere(pos, _footPositionRadius);

        var chest = _chestPosition ? _chestPosition.position : this.transform.position;
        Gizmos.DrawRay(chest, transform.right * _wallRayLength);
        Gizmos.DrawRay(chest, -transform.right * _wallRayLength);
        //Gizmos.DrawRay(, -transform.right * _wallRayLength);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [Header("=== �ݒu���� ===")]
    [SerializeField] Transform _footPosition;
    [SerializeField] float _footPositionRadius = 0.5f;
    [SerializeField] LayerMask _footObjectLayer;
    bool _isGround;

    /// <summary>
    /// �ݒu����p�v���p�e�B
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
    /// �ݒu����
    /// </summary>
    /// <returns></returns>
    bool CheckGround()
    {
        //�����̈ʒu���ݒ肳��Ă��Ȃ���Ύ��M�̍��W��ݒ肷��
        var pos = _footPosition ? _footPosition.position : this.transform.position;
        //�����ɔ���p�̋��̂�p��
        var hit = Physics.OverlapSphere(pos, _footPositionRadius, _footObjectLayer);
        bool check = false;

        if(hit.Length > 0)
        {
            check = true;
        }

        //�A�j���[�V������Bool��ݒ�
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

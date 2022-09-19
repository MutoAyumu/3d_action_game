
using UnityEngine;

/// <summary>
/// ��� IK �𐧌䂷��B
/// </summary>
[RequireComponent(typeof(Animator))]
public class HandIK : MonoBehaviour
{
    /// <summary>�E��̃^�[�Q�b�g</summary>
    [SerializeField] Transform _rightTarget = default;
    /// <summary>����̃^�[�Q�b�g</summary>
    [SerializeField] Transform _leftTarget = default;
    /// <summary>�E��� Position �ɑ΂���E�F�C�g</summary>
    [SerializeField, Range(0f, 1f)] float _rightPositionWeight = 0;
    /// <summary>�E��� Rotation �ɑ΂���E�F�C�g</summary>
    [SerializeField, Range(0f, 1f)] float _rightRotationWeight = 0;
    /// <summary>����� Position �ɑ΂���E�F�C�g</summary>
    [SerializeField, Range(0f, 1f)] float _leftPositionWeight = 0;
    /// <summary>����� Rotation �ɑ΂���E�F�C�g</summary>
    [SerializeField, Range(0f, 1f)] float _leftRotationWeight = 0;
    Animator _anim = default;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        // �E��ɑ΂��� IK ��ݒ肷��
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightPositionWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightRotationWeight);
        _anim.SetIKPosition(AvatarIKGoal.RightHand, _rightTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, _rightTarget.rotation);
        // ����ɑ΂��� IK ��ݒ肷��
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftPositionWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftRotationWeight);
        _anim.SetIKPosition(AvatarIKGoal.LeftHand, _leftTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftHand, _leftTarget.rotation);
    }
}
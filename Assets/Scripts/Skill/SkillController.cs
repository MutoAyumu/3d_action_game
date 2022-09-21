using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    static public BulletSkill _straightBulletSkill;
    static public BulletSkill _homingBulletSkill;
    SpringBoardSkill _springBoardSkill;

    [Header("Transform")]
    [SerializeField] Transform _instancePosition;
    [SerializeField] Transform _footPosition;

    [Header("SkillPrefab")]
    [SerializeField] StraightBullet _straightBullet;
    [SerializeField] HomingBullet _homingBullet;
    [SerializeField] SpringBoard _springBoard;


    private void Awake()
    {
        //�����ɓ����e�X�L���̐���
        _straightBulletSkill = new BulletSkill(0, _straightBullet, _instancePosition);
        //�z�[�~���O�e�X�L���̐���
        _homingBulletSkill = new BulletSkill(1, _homingBullet, _instancePosition);
        //�W�����v��X�L���̐���
        var rb = GetComponent<Rigidbody>();
        _springBoardSkill = new SpringBoardSkill(_springBoard, _footPosition, rb);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            _straightBulletSkill.Use();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            _homingBulletSkill.Use();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            _springBoardSkill.Use();
        }
    }
}

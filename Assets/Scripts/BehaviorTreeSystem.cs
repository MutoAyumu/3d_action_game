using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BehaviorTreeSystem : MonoBehaviour
{
    [SerializeField] List<Branch> _branch = new List<Branch>();
}

/// <summary>
/// �}�̃f�[�^�N���X
/// </summary>
[Serializable]
public class Branch
{
    [SerializeReference, SubclassSelector]
    public List<IConditional> BranchConditionals = new List<IConditional>();

    public Block Block;
}

/// <summary>
/// �u���b�N�̃f�[�^�N���X
/// </summary>
[Serializable]
public class Block
{

}

/// <summary>
/// �ҋ@�����̃f�[�^�N���X
/// </summary>
[Serializable]
public class Queue
{

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BehaviorTreeSystem : MonoBehaviour
{
    [SerializeField] List<Branch> _branch = new List<Branch>();
}

/// <summary>
/// 枝のデータクラス
/// </summary>
[Serializable]
public class Branch
{
    [SerializeReference, SubclassSelector]
    public List<IConditional> BranchConditionals = new List<IConditional>();

    public Block Block;
}

/// <summary>
/// ブロックのデータクラス
/// </summary>
[Serializable]
public class Block
{

}

/// <summary>
/// 待機処理のデータクラス
/// </summary>
[Serializable]
public class Queue
{

}


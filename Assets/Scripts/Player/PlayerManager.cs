using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    static PlayerManager _instance = new PlayerManager();
    public static PlayerManager Instance => _instance;

    //public void Setup(PlayerManagerAttachment attachment)
    //{
    //    attachment.SetupCallback(this)
    //}
}

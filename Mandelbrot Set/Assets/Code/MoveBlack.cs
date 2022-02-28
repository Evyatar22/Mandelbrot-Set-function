using UnityEngine;
using System;

public class MoveBlack : MoveableCircle
{
    protected override void Awake()
    {
        base.Awake();
        
        if (gameObject.name != "BlackCircle")
        {
            Image.color = Color.gray;
            Destroy(Animator);
            enabled = false;
        }
    }
}

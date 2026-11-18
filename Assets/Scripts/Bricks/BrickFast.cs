using System.Collections;
using UnityEngine;

public class BrickFast : ParentBrick
{
    protected override void Activate()
    {
        GlobalManager.Instance.Ball.BrickFast();
    }
}

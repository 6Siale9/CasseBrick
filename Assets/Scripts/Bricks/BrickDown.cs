using UnityEngine;

public class BrickDown : ParentBrick
{
    protected override void Activate()
    {
        GlobalManager.Instance.Ball.NewDir = Vector2.down;
    }
}
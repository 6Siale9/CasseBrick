using UnityEngine;

public class ParentBrick : MonoBehaviour
{
    protected int _hp = 2;

    protected void Start()
    {
        GlobalManager.Instance.Bricks.Add(this);
    }

    public void TakeDamage()
    {
        if (_hp == 2)
        {
            _hp--;
        }
        else if (_hp == 1)
        {
            GlobalManager.Instance.Bricks.Remove(this);
            GlobalManager.Instance.AllSpawnerMove();
            Destroy(gameObject);
        }
    }
}

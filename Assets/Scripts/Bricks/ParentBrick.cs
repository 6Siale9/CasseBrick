using UnityEngine;
using UnityEngine.UI;


public class ParentBrick : MonoBehaviour
{
    protected int _hp = 2;
    [SerializeField] protected Image _img;

    protected void Start()
    {
        GlobalManager.Instance.Bricks.Add(this);
    }

    public void TakeDamage()
    {
        _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, .25f);
        if (_hp == 2)
        {
            _hp--;
        }
        else if (_hp == 1)
        {
            GlobalManager.Instance.Bricks.Remove(this);
            Destroy(gameObject);
            GlobalManager.Instance.AllSpawnerMove();
        }
    }
}

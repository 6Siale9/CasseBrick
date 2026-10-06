using UnityEngine;
using UnityEngine.UI;


public class ParentBrick : MonoBehaviour
{
    protected int _hp = 2;
    protected float _protectedTimer;
    [SerializeField] protected Image _img;

    protected void Start()
    {
        GlobalManager.Instance.Bricks.Add(this);
    }

    protected void Update()
    {
        InvincibilityLogic();
    }

    protected void InvincibilityLogic()
    {
        if (_protectedTimer > 0)
        {
            _protectedTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage()
    {
        if (_protectedTimer <= 0)
        {
            _protectedTimer = 0.2f;
            _img.color = new Color(1 , 1 , 1, .25f);
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
}

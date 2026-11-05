using UnityEngine;
using UnityEngine.UI;


public abstract class ParentBrick : MonoBehaviour
{
    [SerializeField] protected int _hp = 2;
    protected int _maxHp = 2;
    [SerializeField] protected Image _img;
    [SerializeField] protected Color _color;
    [SerializeField] protected Particle _particle;


    protected void Start()
    {
        GlobalManager.Instance.Bricks.Add(this);
        _maxHp = _hp;
        transform.up = new Vector3(1, 1, 0);
    }

    protected void Update()
    {
        DamageLogic();
        OrientationLogic();
    }

    private void OrientationLogic()
    {
        if (transform.up != Vector3.right)
        {
            transform.up = new Vector3(

                transform.up.x,
                Mathf.Clamp(transform.up.y - (Time.deltaTime * 1.25f), 0, 1),
                transform.up.z

                );

        }
    }

    protected void DamageLogic()
    {
        if (_img.color != _color)
        {
            #region R
            if (_img.color.r < _color.r) //+
            {
                _img.color = new Color(

                    Mathf.Clamp(_img.color.r + Time.deltaTime, 0 , _color.r),
                    _img.color.g,
                    _img.color.b,
                    _img.color.a

                    );
            }
            else if (_img.color.r > _color.r) //-
            {
                _img.color = new Color(

                    Mathf.Clamp(_img.color.r - Time.deltaTime, _color.r, 1),
                    _img.color.g,
                    _img.color.b,
                    _img.color.a

                    );
            }
            #endregion R

            #region G
            if (_img.color.g < _color.g) //+
            {
                _img.color = new Color(

                    _img.color.r,
                    Mathf.Clamp(_img.color.g + Time.deltaTime, 0, _color.g),
                    _img.color.b,
                    _img.color.a

                    );
            }
            else if (_img.color.g > _color.g) //-
            {
                _img.color = new Color(

                    _img.color.r,
                    Mathf.Clamp(_img.color.g - Time.deltaTime, _color.g, 1),
                    _img.color.b,
                    _img.color.a

                    );
            }
            #endregion G

            #region B
            if (_img.color.b < _color.b) //+
            {
                _img.color = new Color(

                    _img.color.r,
                    _img.color.g,
                    Mathf.Clamp(_img.color.b + Time.deltaTime, 0, _color.b),
                    _img.color.a

                    );
            }
            else if (_img.color.b > _color.b) //-
            {
                _img.color = new Color(

                    _img.color.r,
                    _img.color.g,
                    Mathf.Clamp(_img.color.b - Time.deltaTime, _color.b, 1),
                    _img.color.a

                    );
            }
            #endregion B

            #region A
            if (_img.color.a < _color.a) //+
            {
                _img.color = new Color(

                    _img.color.r,
                    _img.color.g,
                    _img.color.b,
                    Mathf.Clamp(_img.color.a + Time.deltaTime, 0, _color.a)

                    );
            }
            else if (_img.color.a > _color.a) //-
            {
                _img.color = new Color(

                    _img.color.r,
                    _img.color.g,
                    _img.color.b,
                    Mathf.Clamp(_img.color.a - Time.deltaTime, _color.a, 1)

                    );
            }
            #endregion A
        }
    }
    public void TakeDamage()
    {
        if (_hp > 1)
        {
            _hp--;
            Damage();
        }
        else if (_hp == 1)
        {
            ScoreManager.Instance.Score += ScoreManager.Instance.Mult;
            Particle();
            GlobalManager.Instance.Bricks.Remove(this);
            Destroy(gameObject);
            GlobalManager.Instance.AllSpawnerMove();
        }
        Activate();
    }

    protected void Particle()
    {
        for (int i = 0; i < 15; i++)
        {
            Particle p = Instantiate(_particle, transform.position, Quaternion.identity);
            p.MaxForce = 500f;
            p.MinForce = 50f;
            p.Color = _color;
        }
    }

    protected abstract void Activate();
    protected void Damage()
    {
        float a = _hp;
        float b = _maxHp;
        _color = new Color(
            
            _color.r,
            _color.g,
            _color.b,
            a/b
            
            );
        _img.color = Color.white;
    }

}

using UnityEngine;
using UnityEngine.UI;


public abstract class ParentBrick : MonoBehaviour
{
    [SerializeField] protected int _hp = 2;
    private int _maxHp = 2;
    [SerializeField] private Image _img;
    [SerializeField] protected Color _color;
    [SerializeField] private Particle _particle;

    public Image Img { get => _img; set => _img = value; }

    protected void Start()
    {
        GlobalManager.Instance.Bricks.Add(this);
        _maxHp = _hp;
        Img.rectTransform.up = new Vector3(1, 1, 0);
    }

    protected void Update()
    {
        DamageLogic();
        OrientationLogic();
    }

    private void OrientationLogic()
    {
        if (_img.rectTransform.up != Vector3.right)
        {
            _img.rectTransform.up = new Vector3(

                _img.rectTransform.up.x,
                Mathf.Clamp(_img.rectTransform.up.y - (Time.deltaTime * 1.25f), 0, 1),
                _img.rectTransform.up.z

                );

        }
    }

    private void DamageLogic()
    {
        if (Img.color != _color)
        {
            #region R
            if (Img.color.r < _color.r) //+
            {
                Img.color = new Color(

                    Mathf.Clamp(Img.color.r + Time.deltaTime, 0 , _color.r),
                    Img.color.g,
                    Img.color.b,
                    Img.color.a

                    );
            }
            else if (Img.color.r > _color.r) //-
            {
                Img.color = new Color(

                    Mathf.Clamp(Img.color.r - Time.deltaTime, _color.r, 1),
                    Img.color.g,
                    Img.color.b,
                    Img.color.a

                    );
            }
            #endregion R

            #region G
            if (Img.color.g < _color.g) //+
            {
                Img.color = new Color(

                    Img.color.r,
                    Mathf.Clamp(Img.color.g + Time.deltaTime, 0, _color.g),
                    Img.color.b,
                    Img.color.a

                    );
            }
            else if (Img.color.g > _color.g) //-
            {
                Img.color = new Color(

                    Img.color.r,
                    Mathf.Clamp(Img.color.g - Time.deltaTime, _color.g, 1),
                    Img.color.b,
                    Img.color.a

                    );
            }
            #endregion G

            #region B
            if (Img.color.b < _color.b) //+
            {
                Img.color = new Color(

                    Img.color.r,
                    Img.color.g,
                    Mathf.Clamp(Img.color.b + Time.deltaTime, 0, _color.b),
                    Img.color.a

                    );
            }
            else if (Img.color.b > _color.b) //-
            {
                Img.color = new Color(

                    Img.color.r,
                    Img.color.g,
                    Mathf.Clamp(Img.color.b - Time.deltaTime, _color.b, 1),
                    Img.color.a

                    );
            }
            #endregion B

            #region A
            if (Img.color.a < _color.a) //+
            {
                Img.color = new Color(

                    Img.color.r,
                    Img.color.g,
                    Img.color.b,
                    Mathf.Clamp(Img.color.a + Time.deltaTime, 0, _color.a)

                    );
            }
            else if (Img.color.a > _color.a) //-
            {
                Img.color = new Color(

                    Img.color.r,
                    Img.color.g,
                    Img.color.b,
                    Mathf.Clamp(Img.color.a - Time.deltaTime, _color.a, 1)

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

    private void Particle()
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
    private void Damage()
    {
        float a = _hp;
        float b = _maxHp;
        _color = new Color(
            
            _color.r,
            _color.g,
            _color.b,
            a/b
            
            );
        Img.color = Color.white;
    }

}

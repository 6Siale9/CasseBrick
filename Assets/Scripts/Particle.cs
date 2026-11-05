using UnityEngine;
using UnityEngine.UI;

public class Particle : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _lifeTimeThreshold;
    private float _lifeTime;
    private Rigidbody2D _rb;
    private Color _color = Color.white;

    public float MinForce { get => _minForce; set => _minForce = value; }
    public float MaxForce { get => _maxForce; set => _maxForce = value; }
    public Color Color { get => _color; set => _color = value; }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Impulse();
        _img.color = _color;
    }

    // Update is called once per frame
    void Update()
    {
        SelfDestructLogic();
    }

    private void Impulse()
    {
        Vector2 v = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
            );
        float f = Random.Range(MinForce, MaxForce);
        _rb.AddForce(v.normalized * f);
    }

    private void SelfDestructLogic()
    {
        if (_lifeTime < _lifeTimeThreshold)
        {
            _lifeTime += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

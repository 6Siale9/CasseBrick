using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector2 _up = Vector2.up;
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = _up * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bouncer b = collision.GetComponent<Bouncer>();
        if (b != null)
        {
            float f = Vector2.Angle(_rb.linearVelocity, b.RightAngle);
            Debug.Log(f);
            _rb.linearVelocity = Vector2.Reflect(_rb.linearVelocity, b.RightAngle).normalized * _speed;
            /*
            Debug.Log("Bounce");
            gameObject.transform.up = -gameObject.transform.up;
            _up = gameObject.transform.up;
            _rb.linearVelocity = _up * _speed;
            
            float f = Vector2.Angle(_rb.linearVelocity, b.RightAngle);
            f /= 360f;

            gameObject.transform.rotation = new Quaternion(0,
                                                0,
                                                transform.rotation.z + f,
                                                1);

            Debug.Log(gameObject.transform.rotation);
            Debug.Log(f);
            Debug.Log(gameObject.transform.rotation);
            
            _up = gameObject.transform.up;
            _rb.linearVelocity = _up * _speed;
            */
        }
    }
}

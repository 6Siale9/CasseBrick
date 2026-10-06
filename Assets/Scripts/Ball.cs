using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector2 _up = Vector2.up;
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GlobalManager.Instance.Balls.Add(this);
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
            if (_speed < 50)
            {
                _speed += 0.3f;
                _speed = Mathf.Clamp(_speed, 0, 50);
            }
            float f = Vector2.Angle(_rb.linearVelocity, b.RightAngle);
            Vector2 vector = Vector2.Reflect(_rb.linearVelocity, b.RightAngle).normalized * _speed;
            vector = Rotate(vector, Random.Range(-0.1f, 0.1f));
            _rb.linearVelocity = vector;
            b.Activate();
        }
    }

    public void SeekingBall(ParentBrick brick)
    {
        Vector2 origin = gameObject.transform.position;
        Vector2 target = brick.gameObject.transform.position;
        Vector2 dir = target - origin;
        _rb.linearVelocity = dir.normalized * _speed;
    }

    private Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}

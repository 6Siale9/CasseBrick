using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector2 _up = Vector2.up;
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private GameObject _child;
    [SerializeField] private int _howManyRays;
    private List<Ray2D> _rays = new List<Ray2D>(); 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GlobalManager.Instance.Balls.Add(this);
        _rb = GetComponent<Rigidbody2D>();
        //_rb.linearVelocity = _up * _speed;
        Vector2 v = new Vector2(Random.Range(-1f, 1), 1).normalized * 100;
        v = new Vector2(1, 1);
        DrawRays(transform.position, v);
    }

    // Update is called once per frame
    void Update()
    {
        ChildTransformLogic();
    }

    private void DrawRays(Vector2 origin, Vector2 direction)
    {
        RaycastHit2D[] rcH = Physics2D.RaycastAll(origin, direction);
        Ray2D r = new Ray2D(origin, direction);
        _rays.Add(r);

        Vector2 collisionPoint = Vector2.zero;
        Vector2 reflectedDir = Vector2.zero;
        if (rcH.Length > 0 && _howManyRays > 0)
        {
            _howManyRays--;
            for (int i = 0; i < rcH.Length; i++)
            {
                Bouncer b = rcH[i].collider.GetComponent<Bouncer>();
                if (b != null)
                {
                    collisionPoint = rcH[i].point;
                    reflectedDir = Vector2.Reflect(direction, b.transform.up);
                    break;
                }
            }
            DrawRays(collisionPoint, reflectedDir);
        }
        else
        {
            Finished();
        }
    }

    private void Finished()
    {
        foreach (Ray2D ray in _rays)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10);
        }
        Debug.Log("Finished");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bouncer b = collision.GetComponent<Bouncer>();
        if (b != null)
        {
            if (_speed < _maxSpeed)
            {
                _speed += 0.3f;
                _speed = Mathf.Clamp(_speed, 0, _maxSpeed);
            }
            float f = Vector2.Angle(_rb.linearVelocity, b.RightAngle);
            Vector2 vector = Vector2.Reflect(_rb.linearVelocity, b.RightAngle).normalized * _speed;
            vector = Rotate(vector, Random.Range(-0.1f, 0.1f));
            _rb.linearVelocity = vector;
            b.Activate();
        }
    }

    private void ChildTransformLogic()
    {
        _child.transform.up = _rb.linearVelocity.normalized;
        _child.transform.localScale = new Vector3(1, 1 + _speed/15, 1);
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

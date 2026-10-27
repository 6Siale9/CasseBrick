using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector2 _up = Vector2.up;
    //private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private GameObject _child;
    private bool _squash = false;
    private Vector2 _squashDir = Vector2.zero;
    private List<Ray2D> _rays = new List<Ray2D>();
    private Vector2 _destination;
    private Vector2 _dir;
    private Vector2 _newDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GlobalManager.Instance.Balls.Add(this);
        //_rb = GetComponent<Rigidbody2D>();
        //_rb.linearVelocity = _up * _speed;


        Vector2 v = new Vector2(Random.Range(-1f, 1), 1).normalized * 100;
        v = new Vector2(1, 1);
        NewDest(transform.position, v);
    }

    // Update is called once per frame
    void Update()
    {
        ChildTransformLogic();
        AdvanceLogic();
    }

    private void AdvanceLogic()
    {
        if (transform.position.x == _destination.x && transform.position.y == _destination.y)
        {
            NewDest(transform.position, _newDir);
        }
        else
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _destination, step);
        }
    }

    protected void ChildTransformLogic()
    {
        if (!_squash)
        {
            _child.transform.up = _dir.normalized;
            _child.transform.localScale = new Vector3(1, 1 + _speed / 15, 1);
        }
        else
        {
            _child.transform.up = _squashDir;
            _child.transform.localScale = new Vector3(1 + _speed / 15, 1, 1);
        }
    }
    private void NewDest(Vector2 origin, Vector2 direction)
    {
        _speed += 8;
        _speed = Mathf.Clamp(_speed, 0, _maxSpeed);
        _dir = direction;
        RaycastHit2D[] rcH = Physics2D.RaycastAll(origin, direction);
        Ray2D r = new Ray2D(origin, direction);
        _rays.Add(r);
        Vector2 reflectedDir = Vector2.zero;
        if (rcH.Length > 0)
        {
            for (int i = 0; i < rcH.Length; i++)
            {
                Bouncer b = rcH[i].collider.GetComponent<Bouncer>();
                if (b != null)
                {
                    if (rcH[i].point != _destination)
                    {
                        _destination = rcH[i].point;
                        _newDir = Vector2.Reflect(direction, b.transform.up);
                        //_newDir = Rotate(_newDir, Random.Range(-0.1f, 0.1f));
                        break;
                    }
                }
            }
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bouncer b = collision.GetComponent<Bouncer>();
        if (b != null)
        {
            if (_speed < _maxSpeed)
            {
                _speed += 3f;
                _speed = Mathf.Clamp(_speed, 0, _maxSpeed);
            }
            //float f = Vector2.Angle(_rb.linearVelocity, b.RightAngle);
            //Vector2 vector = Vector2.Reflect(_rb.linearVelocity, b.RightAngle).normalized * _speed;
            //vector = Rotate(vector, Random.Range(-0.1f, 0.1f));
            //_rb.linearVelocity = vector;
            b.Activate();
            StartCoroutine(Stop((_speed * _speed) * 0.0001f));
            _squashDir = b.RightAngle;
        }
    }
    */

    public void SeekingBall(ParentBrick brick) // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        Vector2 origin = gameObject.transform.position;
        Vector2 target = brick.gameObject.transform.position;
        Vector2 dir = target - origin;
        //_rb.linearVelocity = dir.normalized * _speed;
    }

    private Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}

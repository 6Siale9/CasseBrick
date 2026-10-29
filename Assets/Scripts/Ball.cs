using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private GameObject _child;
    private Vector2 _squashDir = Vector2.zero;
    private List<Ray2D> _rays = new List<Ray2D>();
    private Vector2 _destination;
    private Vector2 _dir;
    private Vector2 _newDir;
    private Bouncer _target;
    private bool _stop = false;
    private float _stopTime = 0;
    private float _stopTimeThreshold = 0;
    private int _bounce = 0;
    [SerializeField] private int _bounceThreshold;
    public float StopTime { get => _stopTime; set => _stopTime = value; }
    public float StopTimeThreshold { get => _stopTimeThreshold; set => _stopTimeThreshold = value; }
    public float Speed { get => _speed; set => _speed = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GlobalManager.Instance.Ball = this;
        Vector2 v = new Vector2(Random.Range(-1f, 1), 1).normalized * 100;
        v = new Vector2(0, -1);
        NewDest(transform.position, v);
    }

    // Update is called once per frame
    void Update()
    {
        ChildTransformLogic();
        AdvanceLogic();
        StopLogic();
    }
    private void StopLogic()
    {
        if (_stop)
        {
            if (_stopTime < _stopTimeThreshold)
            {
                _stopTime += Time.deltaTime;
            }
            else
            {
                _stopTime = 0;
                _stop = false;
            }
        }
    }

    private void AdvanceLogic()
    {
        if (!_stop)
        {
            if (transform.position.x == _destination.x && transform.position.y == _destination.y)
            {
                _squashDir = _target.RightAngle;
                _target.Activate();
                if (_target.Stop)
                {
                    _stopTimeThreshold = Speed * 5 / _maxSpeed; // ?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
                }
                else
                {
                    _stopTimeThreshold = .1f; // ???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
                }
                _stop = true;
                if (_bounce < _bounceThreshold)
                {
                    if (_target.AddBounce)
                    {
                        _bounce++;
                    }
                    else
                    {
                        _bounce = 0;
                    }
                    NewDest(transform.position, _newDir);
                }
                else
                {
                    _bounce = 0;
                    if (GlobalManager.Instance.Bricks.Count != 0)
                    {
                        Vector2 origin = transform.position;
                        Vector2 destination = GlobalManager.Instance.Bricks[Random.Range(0, GlobalManager.Instance.Bricks.Count)].gameObject.transform.position;
                        Vector2 dir = destination - origin;
                        NewDest(transform.position, dir);
                    }
                    else
                    {
                        NewDest(transform.position, _newDir);
                    }
                }
            }
            else
            {
                float step = Speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, _destination, step);
            }
        }
    }

    protected void ChildTransformLogic()
    {
        if (!_stop)
        {
            _child.transform.up = _dir.normalized;
            _child.transform.localScale = new Vector3(1, 1 + Speed / 25, 1);
        }
        else
        {
            _child.transform.up = _squashDir;
            _child.transform.localScale = new Vector3(2.5f, 1, 1);
        }
    }

    public void NewSpeed(float value, bool addOn)
    {
        if (addOn)
        {
            Speed += value;
        }
        else
        {
            Speed = value;
        }
        Speed = Mathf.Clamp(Speed, 0, _maxSpeed);
    }

    private void NewDest(Vector2 origin, Vector2 direction)
    {
        _dir = direction;
        RaycastHit2D[] rcH = Physics2D.RaycastAll(origin, direction);
        Ray2D r = new Ray2D(origin, direction);
        _rays.Add(r);
        Vector2 reflectedDir = Vector2.zero;
        bool center = true;
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
                        _newDir = Rotate(_newDir, Random.Range(-0.1f, 0.1f));
                        center = false;
                        _target = b;
                        break;
                    }
                }
            }
            if (center)
            {
                GoToCenter();
            }
        }
        else
        {
            GoToCenter();
        }
    }

    private void GoToCenter()
    {
        Vector2 origin = transform.position;
        Vector2 dir = new Vector2(0, 0) - origin;

        NewDest(origin, dir);
    }

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
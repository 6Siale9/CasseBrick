using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI;

public class Ball : MonoBehaviour
{
    #region Attribut
    [Header("Movement")]
    [SerializeField] private float _speed; //Speed of the ball
    [SerializeField] private float _maxSpeed; //Maximum speed after bounces
    private Vector2 _destination; //Point of Raycast
    private Vector2 _dir; //Direcction to _destination for stepping
    private Vector2 _newDir; //Next direction after bounce
    private Bouncer _target; //Target to activate on bounce
    private Vector2 _normal; //Normal of _target
    
    [Header("StretchAndSquash")]
    [SerializeField] private GameObject _child; //Object to stretch
    private Vector2 _squashDir = Vector2.zero; //Orientation of stretch (Wall normal or _dir)
    
    [Header("Hitstop")]
    private bool _stop = true; //Allow the ball to move
    private float _stopTime = 0; //Value
    private float _stopTimeThreshold = 999; //Time the ball stops upon colliding
    [SerializeField] private float _stopTimeThresholdFast = 0; //Value for _stopTimeThreshold for object with fast rebounce

    [Header("SeekingBall")]
    private int _bounce = 0; //Number of bounces since last collision with a brick
    [SerializeField] private int _bounceThreshold; //Number of bounces required for SeekingBall()

    [Header("Particles")]
    [SerializeField] private Particle _particle;

    [Header("Other")]
    [SerializeField] private float _waitAtSpawn; //Time before the ball moves at spawn
    [SerializeField] private float _shakeThreshold; //Speed needed for cameraShake

    #endregion Attribut

    public float StopTime { get => _stopTime; set => _stopTime = value; }
    public float StopTimeThreshold { get => _stopTimeThreshold; set => _stopTimeThreshold = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public int Bounce { get => _bounce; set => _bounce = value; }
    public Vector2 NewDir { get => _newDir; set => _newDir = value; }
    public Bouncer Target { get => _target; set => _target = value; }
    public float StopTimeThresholdFast { get => _stopTimeThresholdFast; set => _stopTimeThresholdFast = value; }

    #region Method
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        ChildTransformLogic();
        AdvanceLogic();
        StopLogic();
    }

    #region Logic
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
            if (transform.position.x == _destination.x && transform.position.y == _destination.y) //Activate if ball is at destination
            {
                _squashDir = _normal;
                Target.Activate();
                Particle();
                if (_speed >= _shakeThreshold)
                {
                    CameraManager.Instance.CameraShake(_normal);
                }

                #region Stop
                if (_bounce == _bounceThreshold -1 && Target.AddBounce)
                {
                    _stopTimeThreshold = Mathf.Clamp(Speed * 15f / _maxSpeed, 0f, 4.5f); // Big rebounce for seeking ball ??????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? 
                    _stop = true;
                }
                else if (Target.Stop)
                {
                    _stopTimeThreshold = Mathf.Clamp(Speed * 5f / _maxSpeed, 0f, 1.5f); // Pause for long rebounce ?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
                }
                else
                {
                    _stopTimeThreshold = StopTimeThresholdFast; // Pause for fast rebounce
                }
                if (_stopTimeThreshold >= StopTimeThresholdFast)
                {
                    _stop = true;
                }
                #endregion Stop

                #region Bounce
                if (Target.AddBounce)
                {
                    Bounce++;
                }
                else
                {
                    Bounce = 0;
                }

                if (Bounce < _bounceThreshold)
                {
                    NewDest(transform.position, NewDir);
                }
                else
                {
                    if (GlobalManager.Instance.Bricks.Count != 0)
                    {
                        Vector2 origin = transform.position;
                        Vector2 destination = GlobalManager.Instance.Bricks[Random.Range(0, GlobalManager.Instance.Bricks.Count)].gameObject.transform.position;
                        Vector2 dir = destination - origin;
                        NewDest(transform.position, dir);
                    }
                    else
                    {
                        NewDest(transform.position, NewDir);
                    }
                }
                #endregion Bounce
            }
            else
            {
                float step = Speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, _destination, step); // Step
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
    #endregion Logic

    private void Initialize()
    {
        GlobalManager.Instance.Ball = this;
        Vector2 v = new Vector2(Random.Range(-1f, 1), Random.Range(-1f, 1)).normalized * 100;
        StartCoroutine(WaitAtSpawn(v));
    }

    private IEnumerator WaitAtSpawn(Vector2 v)
    {
        yield return new WaitForSeconds(_waitAtSpawn);
        NewDest(transform.position, v);
        _stop = false;
    }

    private void NewDest(Vector2 origin, Vector2 direction)
    {
        _dir = direction;
        RaycastHit2D[] rcH = Physics2D.CircleCastAll(origin, .01f, direction);
        Ray2D r = new Ray2D(origin, direction);
        Vector2 reflectedDir = Vector2.zero;
        bool center = true;
        if (rcH.Length > 0)
        {
            for (int i = 0; i < rcH.Length; i++)
            {
                Bouncer b = rcH[i].collider.GetComponent<Bouncer>();
                if (b != null)
                {
                    float f = Vector2.Distance(rcH[i].point, transform.position);
                    if (f > 0.02)
                    {
                        _destination = rcH[i].point;
                        _normal = rcH[i].normal;
                        NewDir = Vector2.Reflect(direction, _normal);
                        NewDir = Rotate(NewDir, Random.Range(-0.1f, 0.1f));
                        center = false;
                        Target = b;
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

    public void Rebound()
    {
        _squashDir = _normal;
        Target.Activate();
        Particle();

        #region Stop
        if (_bounce == _bounceThreshold - 1 && Target.AddBounce)
        {
            _stopTimeThreshold = Speed * 30 / _maxSpeed; // Big rebounce for seeking ball ??????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? 
            _stop = true;
        }
        else if (Target.Stop)
        {
            _stopTimeThreshold = Speed * 5 / _maxSpeed; // Pause for long rebounce ?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
        }
        else
        {
            _stopTimeThreshold = StopTimeThresholdFast; // Pause for fast rebounce
        }
        if (_stopTimeThreshold >= StopTimeThresholdFast)
        {
            _stop = true;
        }
        #endregion Stop

        #region Bounce
        if (Target.AddBounce)
        {
            Bounce++;
        }
        else
        {
            Bounce = 0;
        }

        if (Bounce < _bounceThreshold)
        {
            NewDest(transform.position, NewDir);
        }
        else
        {
            if (GlobalManager.Instance.Bricks.Count != 0)
            {
                Vector2 origin = transform.position;
                Vector2 destination = GlobalManager.Instance.Bricks[Random.Range(0, GlobalManager.Instance.Bricks.Count)].gameObject.transform.position;
                Vector2 dir = destination - origin;
                NewDest(transform.position, dir);
            }
            else
            {
                NewDest(transform.position, NewDir);
            }
        }
        #endregion Bounce
    }

    private void GoToCenter() //Failsafe method if Raycast fails to get out of a corner
    {
        Vector2 origin = transform.position;
        Vector2 dir = new Vector2(0, 0) - origin;

        NewDest(origin, dir);
    }

    private void Particle()
    {
        for (int i = 0; i < 2; i++)
        {
            Particle p = Instantiate(_particle, transform.position, Quaternion.identity);
            p.MaxForce = 200f;
            p.MinForce = 20f;
        }
    }

    #region Tool
    private Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    #endregion Tool
    #endregion Method
}
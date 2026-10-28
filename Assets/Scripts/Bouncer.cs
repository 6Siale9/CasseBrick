using System.Collections.Specialized;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private ParentBrick _brick;
    [SerializeField] private Palet _palet;
    [SerializeField] private bool _stop = false;
    private Vector2 _rightAngle;

    public Vector2 RightAngle { get => _rightAngle; set => _rightAngle = value; }
    public bool Stop { get => _stop; set => _stop = value; }

    void Start()
    {
        _rightAngle = -gameObject.transform.up;
        if (_brick != null)
        {
            Stop = true;
        }
    }

    public void Activate()
    {
        if (_brick != null)
        {
            _brick.TakeDamage();
            GlobalManager.Instance.ResetBounce();
        }
        else if (_palet != null)
        {
            _palet.Bounce();
        }
        else
        {
            GlobalManager.Instance.AddBounce();
        }
    }
}

using System.Collections.Specialized;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private ParentBrick _brick;

    private Vector2 _rightAngle;

    public Vector2 RightAngle { get => _rightAngle; set => _rightAngle = value; }

    void Start()
    {
        _rightAngle = -gameObject.transform.up;
    }

    public void Activate()
    {
        if (_brick != null)
        {
            _brick.TakeDamage();
            GlobalManager.Instance.ResetBounce();
        }
        else
        {
            GlobalManager.Instance.AddBounce();
        }
    }
}

using System.Collections.Specialized;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    private Vector2 _rightAngle;

    public Vector2 RightAngle { get => _rightAngle; set => _rightAngle = value; }

    void Start()
    {
        _rightAngle = -gameObject.transform.up;
    }
}

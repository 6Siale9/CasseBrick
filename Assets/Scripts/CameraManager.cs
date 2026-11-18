using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Instance
    private static CameraManager _instance;
    public static CameraManager Instance { get => _instance; set => _instance = value; }

    private void TrySetInstance()
    {
        if (CameraManager.Instance == null)
        {
            CameraManager.Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion Instance

    private Vector3 _shakeDir;
    private float _shakeValue;
    [SerializeField] private float _shakeValueThreshold;
    [SerializeField] private float _shakeMult;


    private void Awake()
    {
        TrySetInstance();
    }

    private void Update()
    {
        ShakeLogic();
    }

    private void ShakeLogic()
    {
        if (_shakeValue > 0)
        {
            _shakeValue = Mathf.Clamp(_shakeValue - Time.deltaTime * _shakeMult, 0f, 999f);
            transform.localPosition = _shakeDir * _shakeValue;
        }
    }

    public void CameraShake(Vector2 dir)
    {
        _shakeDir = new Vector3(dir.normalized.x, dir.normalized.y, 0f);
        _shakeValue = _shakeValueThreshold;
    }
}

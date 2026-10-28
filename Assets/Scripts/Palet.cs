using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Palet : MonoBehaviour
{
    [SerializeField] private float _cd = 0;
    [SerializeField] private float _activeTimeThreshold = 0;
    [SerializeField] private float _perfectTimeThreshold = 0;
    private float _activeTime = 0;
    private EpaletState _state = EpaletState.INACTIVE;
    [SerializeField] private Color _perfectColor = Color.white;
    [SerializeField] private Color _activeColor = Color.white;
    [SerializeField] private Color _cdColor = Color.white;
    [SerializeField] private Color _inactiveColor = Color.white;
    [SerializeField] private Image _image;
    [SerializeField] private int _score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_state != EpaletState.INACTIVE)
        {
            ActiveLogic();
        }
        ColorLogic();
        InputLogic();
    }

    private void InputLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _state == EpaletState.INACTIVE)
        {
            _state = EpaletState.PERFECT;
        }
    }

    private void ColorLogic()
    {
        switch (_state)
        {
            default:
            case EpaletState.PERFECT:
                _image.color = _perfectColor;
                break;
            case EpaletState.ACTIVE:
                _image.color = _activeColor;
                break;
            case EpaletState.ONCOOLDOWN:
                _image.color = _cdColor;
                break;
            case EpaletState.INACTIVE:
                _image.color = _inactiveColor;
                break;
        }
    }

    private void ActiveLogic()
    {
        switch (_state)
        {
            default:
            case EpaletState.PERFECT:
                if (_activeTime < _perfectTimeThreshold)
                {
                    _activeTime += Time.deltaTime;
                }
                else
                {
                    _activeTime = 0;
                    _state = EpaletState.ACTIVE;
                }
                break;
            case EpaletState.ACTIVE:
                if (_activeTime < _activeTimeThreshold)
                {
                    _activeTime += Time.deltaTime;
                }
                else
                {
                    _activeTime = 0;
                    _state = EpaletState.ONCOOLDOWN;
                }
                break;
            case EpaletState.ONCOOLDOWN:
                if (_activeTime < _cd)
                {
                    _activeTime += Time.deltaTime;
                }
                else
                {
                    _activeTime = 0;
                    _state = EpaletState.INACTIVE;
                }
                break;
            case EpaletState.INACTIVE:
                Debug.Log("Error in Palet script in switch at ActiveLogic()");
                break;
        }
    }

    public void Bounce()
    {
        foreach (Ball ball in GlobalManager.Instance.Balls)
        {
            switch (_state)
            {
                default:
                case EpaletState.ACTIVE:
                    ball.NewSpeed(1, true);
                    break;
                case EpaletState.PERFECT:
                    ball.NewSpeed(6, true);
                    _score += 1;
                    break;
                case EpaletState.INACTIVE:
                case EpaletState.ONCOOLDOWN:
                    ball.NewSpeed(6, false);
                    _score -= 1;
                    break;
            }
        }
        _state = EpaletState.INACTIVE;
        _activeTime = 0;
    }
}

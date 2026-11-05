using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Palet : MonoBehaviour
{
    [Header("Status")]
    private EpaletState _state = EpaletState.INACTIVE; //Current status
    [SerializeField] private float _perfectTimeThreshold = 0; //Threshold for Perfect
    [SerializeField] private float _activeTimeThreshold = 0; //Threshold for Active
    [SerializeField] private float _cd = 0; //Threshold for OnCooldown
    private float _activeTime = 0; //Value
    private float _savedValue = 0; //For CoyoteTime
    
    [Header("Visible")]
    [SerializeField] private Color _perfectColor = Color.white; //When the palette is in perfect state
    [SerializeField] private Color _activeColor = Color.white; //When the palette is active
    [SerializeField] private Color _cdColor = Color.white; //When the palette is on cooldown
    [SerializeField] private Color _inactiveColor = Color.white; //When the palette is ready
    [SerializeField] private Image _image; //The palette image
    
    [Header("Bars")]
    [SerializeField] private List<Image> _bars = new List<Image>(); //Bars to fill on hitstop
    
    [Header("Effects")]
    [SerializeField] private Image _backgroundD; //Image for damage
    [SerializeField] private float _hurtTimeThreshold; //Time for red flash
    private float _hurtTime = 999; //Value
    [SerializeField] private Image _backgroundP; //Image for perfect
    [SerializeField] private float _pTimeThreshold; //Time for white flash
    private float _pTime = 999; //Value

    #region method
    void Start()
    {
        _savedValue = _perfectTimeThreshold;
    }

    void Update()
    {
        if (_state != EpaletState.INACTIVE)
        {
            ActiveLogic();
        }
        ColorLogic();
        InputLogic();
        SliderLogic();
        BackGroundColorLogic();
    }

    private void BackGroundColorLogic()
    {
        if (_hurtTime < _hurtTimeThreshold)
        {
            _hurtTime += Time.deltaTime;
            _backgroundD.color = new Color(0.4f, 0, 0, _hurtTimeThreshold - _hurtTime);
        }
        if (_pTime < _pTimeThreshold)
        {
            _pTime += Time.deltaTime;
            _backgroundP.color = new Color(1, 1, 1, _pTimeThreshold - _pTime);
        }
    }

    private void SliderLogic()
    {
        foreach (Image img in _bars)
        {
            img.fillAmount = GlobalManager.Instance.Ball.StopTime / GlobalManager.Instance.Ball.StopTimeThreshold;
        }
    }

    private void InputLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_state == EpaletState.INACTIVE)
            {
                _state = EpaletState.PERFECT;
                ActiveLogic();
                CheckForBall();
            }
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

    private void CheckForBall()
    {
        if (GlobalManager.Instance.Ball.transform.position.y <= -6.25f && GlobalManager.Instance.Ball.Target.Palet != null)
        {
            _perfectTimeThreshold = 999f;
        }
    }

    public void Bounce()
    {
        switch (_state)
        {
            default:
            case EpaletState.ACTIVE:
                GlobalManager.Instance.Ball.Speed += 3;
                ScoreManager.Instance.Score += 5;
                ScoreManager.Instance.ScoreShakeTime = 0;
                break;
            case EpaletState.PERFECT:
                GlobalManager.Instance.Ball.Speed += 3;
                ScoreManager.Instance.Score += 5;
                ScoreManager.Instance.ScoreShakeTime = 0;
                ScoreManager.Instance.Mult += 1;
                _pTime = 0;
                break;
            case EpaletState.INACTIVE:
            case EpaletState.ONCOOLDOWN:
                GlobalManager.Instance.Ball.Speed = 5f;
                ScoreManager.Instance.Score = 0;
                ScoreManager.Instance.ScoreShakeTime = 0;
                ScoreManager.Instance.Mult = 0;
                _hurtTime = 0;
                break;
        }
        _perfectTimeThreshold = _savedValue;
        _state = EpaletState.INACTIVE;
        _activeTime = 0;
    }
    #endregion Method
}

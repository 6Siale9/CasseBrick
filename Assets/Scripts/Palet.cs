using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

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
    [SerializeField] private float _score = 0;
    [SerializeField] private List<Image> _sliders = new List<Image>();
    [SerializeField] private Image _background;
    [SerializeField] private float _hurtTimeThreshold;
    private float _hurtTime;
    [SerializeField] private TMP_Text _scoreTxt;
    private float _scoreShakeDeltaX = 0;
    private float _scoreShakeDeltaY = 0;
    private float _scoreShakeTime = 0;
    [SerializeField] private float _scoreShakeTimeThreshold = 0;
    
    
    
    
    
    
    
    
    [SerializeField] private float _mult = 0;







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
        SliderLogic();
        TxtLogic();
        BackGroundColorLogic();
    }

    private void TxtLogic()
    {
        float s = Mathf.Round(_score);
        _scoreTxt.text = s.ToString();

        if (_scoreShakeTime < _scoreShakeTimeThreshold)
        {
            _scoreShakeTime += Time.deltaTime;
            float f = _score * .02f;
            _scoreShakeTimeThreshold = Mathf.Clamp(

                _score * .02f,
                0.01f,
                999

                );
            

            if (_scoreTxt.rectTransform.localPosition.x < _scoreShakeDeltaX)
            {
                _scoreTxt.rectTransform.localPosition = new Vector2(
                    
                    Mathf.Clamp(_scoreTxt.rectTransform.localPosition.x + (Time.deltaTime * _score * 1f), -1, _scoreShakeDeltaX),
                    _scoreTxt.rectTransform.localPosition.y
                    
                    );

            }
            else if (_scoreTxt.rectTransform.localPosition.x > _scoreShakeDeltaX)
            {
                _scoreTxt.rectTransform.localPosition = new Vector2(
                    
                    Mathf.Clamp(_scoreTxt.rectTransform.localPosition.x - (Time.deltaTime  * _score * 1f), _scoreShakeDeltaX, 1),
                    _scoreTxt.rectTransform.localPosition.y
                    
                    );

            }
            else
            {
                _scoreShakeDeltaX = Random.Range(-_score / 100f, _score / 100f);
            }


            if (_scoreTxt.rectTransform.localPosition.y < _scoreShakeDeltaY)
            {
                _scoreTxt.rectTransform.localPosition = new Vector2(

                    _scoreTxt.rectTransform.localPosition.x,
                    Mathf.Clamp(_scoreTxt.rectTransform.localPosition.y + (Time.deltaTime * _score * 1f), -1, _scoreShakeDeltaY)
                    
                    ); 
            }
            else if (_scoreTxt.rectTransform.localPosition.y > _scoreShakeDeltaY)
            {
                _scoreTxt.rectTransform.localPosition = new Vector2(
                    
                    _scoreTxt.rectTransform.localPosition.x,
                    Mathf.Clamp(_scoreTxt.rectTransform.localPosition.y - (Time.deltaTime * _score * 1f), _scoreShakeDeltaY, 1)
                    
                    );
            }
            else
            {
                _scoreShakeDeltaY = Random.Range(-_score / 100f, _score / 100f);
            }
        }
        else
        {
            _scoreTxt.rectTransform.localPosition = new Vector2(0, 0);
        }
    }

    private void BackGroundColorLogic()
    {
        if (_hurtTime < _hurtTimeThreshold)
        {
            _hurtTime += Time.deltaTime;
            _background.color = new Color(0.4f, 0, 0, _hurtTimeThreshold - _hurtTime);
        }
    }

    private void SliderLogic()
    {
        foreach (Image img in _sliders)
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

    public void Bounce()
    {
        switch (_state)
        {
            default:
            case EpaletState.ACTIVE:
                GlobalManager.Instance.Ball.NewSpeed(1, true);
                _score += 1;
                _scoreShakeTime = 0;
                break;
            case EpaletState.PERFECT:
                GlobalManager.Instance.Ball.NewSpeed(6, true);
                _score += 10;
                _scoreShakeTime = 0;
                break;
            case EpaletState.INACTIVE:
            case EpaletState.ONCOOLDOWN:
                GlobalManager.Instance.Ball.Speed /= 1.5f;
                _score /= 1.5f;
                _hurtTime = 0;
                _scoreShakeTime = 0;
                break;
        }
        _state = EpaletState.INACTIVE;
        _activeTime = 0;
    }
}

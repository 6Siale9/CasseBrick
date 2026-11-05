using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BounceManager : MonoBehaviour
{
    [SerializeField] private List<Image> _imgs = new List<Image>();
    [SerializeField] private List<Image> _backgrounds = new List<Image>();
    [SerializeField] private Ball _ball;
    private int _current = 9;
    private bool _shake = true;
    private List<float> _shakeX = new List<float>();
    private List<float> _shakeY = new List<float>();
    [SerializeField] private float _speedMult;
    [SerializeField] private float _mult;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        SquareLogic();
        ShakeLogic();
    }

    private void Initialize()
    {
        foreach (Image img in _imgs)
        {
            _shakeX.Add(0);
            _shakeY.Add(0);
        }
        foreach (Image img in _backgrounds)
        {
            _shakeX.Add(0);
            _shakeY.Add(0);
        }
    }

    private void SquareLogic()
    {

        if (_current != _ball.Bounce)
        {
            if (_current < _ball.Bounce)
            {
                switch (_current)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        _imgs[_current].enabled = true;
                        break;
                    case 6:
                        _imgs[_current].enabled = true;
                        _shake = true;
                        break;
                }
                _current++;
            }
            else if (_ball.Bounce == 0)
            {
                _current = 0;
                _shake = false;
                for (int i = 0; i < _imgs.Count; i++)
                {
                    _imgs[i].transform.localPosition = Vector3.zero;
                    _imgs[i].enabled = false;

                }
                for (int i = 0; i < _backgrounds.Count; i++)
                {
                    _backgrounds[i].transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    private void ShakeLogic()
    {
        if (_shake)
        {
            for (int i = 0; i < _imgs.Count; i++)
            {
                Shake(_imgs[i], i);
            }
            for (int i = 0; i < _backgrounds.Count; i++)
            {
                Shake(_backgrounds[i], i + _imgs.Count);
            }
        }
    }

    private void Shake(Image img, int i)
    {
        if (img.rectTransform.localPosition.x < _shakeX[i])
        {
            img.rectTransform.localPosition = new Vector2(

                Mathf.Clamp(img.rectTransform.localPosition.x + (Time.deltaTime * _speedMult * 1f), -1, _shakeX[i]),
                
                img.rectTransform.localPosition.y

                );

        }
        else if (img.rectTransform.localPosition.x > _shakeX[i])
        {
            img.rectTransform.localPosition = new Vector2(

                Mathf.Clamp(img.rectTransform.localPosition.x - (Time.deltaTime * _speedMult * 1f), _shakeX[i], 1),

                img.rectTransform.localPosition.y

                );

        }
        else
        {
            _shakeX[i] = Random.Range(-_mult / 100f, _mult / 100f);
        }




        if (img.rectTransform.localPosition.y < _shakeY[i])
        {
            img.rectTransform.localPosition = new Vector2(

                
                img.rectTransform.localPosition.x,

                Mathf.Clamp(img.rectTransform.localPosition.y + (Time.deltaTime * _speedMult * 1f), -1, _shakeY[i])

                );

        }
        else if (img.rectTransform.localPosition.y > _shakeY[i])
        {
            img.rectTransform.localPosition = new Vector2(

                img.rectTransform.localPosition.x,

                Mathf.Clamp(img.rectTransform.localPosition.y - (Time.deltaTime * _speedMult * 1f), _shakeY[i], 1)

                );

        }
        else
        {
            _shakeY[i] = Random.Range(-_mult / 100f, _mult / 100f);
        }
    }
}

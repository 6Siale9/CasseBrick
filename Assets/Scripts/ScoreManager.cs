using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Instance
    private static ScoreManager _instance;
    public static ScoreManager Instance { get => _instance; set => _instance = value; }

    private void TrySetInstance()
    {
        if (ScoreManager.Instance == null)
        {
            ScoreManager.Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion Instance

    private float _score = 0; //Score of the player
    private float _bestScore = 0; //Highest value of _score this game
    [SerializeField] private TMP_Text _scoreTxt; //TMP_Text for _score
    [SerializeField] private TMP_Text _bestScoreTxt; //TMP_Text for _bestScore
    private float _scoreShakeDeltaX = 0; //Value
    private float _scoreShakeDeltaY = 0; //Value
    private float _scoreShakeTime = 0; //Value
    [SerializeField] private float _scoreShakeTimeThreshold = 0; //How long the text will shake

    public float Score { get => _score; set => _score = value; }
    public float ScoreShakeTime { get => _scoreShakeTime; set => _scoreShakeTime = value; }

    private void Awake()
    {
        TrySetInstance();
    }

    private void Update()
    {
        TxtLogic();
    }
    private void TxtLogic()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
        }

        float s = Mathf.Round(_score);
        float bs = Mathf.Round(_bestScore);
        _bestScoreTxt.text = bs.ToString();
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

                    Mathf.Clamp(_scoreTxt.rectTransform.localPosition.x - (Time.deltaTime * _score * 1f), _scoreShakeDeltaX, 1),
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
}

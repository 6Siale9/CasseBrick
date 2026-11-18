using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private int _txtNumber = 0;
    private int _lineNumber = 0;
    private int _letterNumber = 0;
    [SerializeField] private List<int> _dialogLengths;
    [SerializeField] private List<string> _txts;
    private string _currentTxt = "";
    private string _toCopyText;
    [SerializeField] private float _timeBetweenLettersThreshold;
    [SerializeField] private float _timeBetweenLines;
    private float _timeBetweenLettersValue;
    [SerializeField] private TMP_Text _txtUi;
    private bool _go = true;

    private struct Sdialog
    {
        private List<string> _lists;

        public List<string> Lists { get => _lists; set => _lists = value; }

        public Sdialog(List<string> list)
        {
            _lists = list;
        }
    }
    private List<Sdialog> _dialogs = new List<Sdialog>();

    private void Start()
    {
        Initiate();
    }

    void Update()
    {
        if (_go)
        {
            TextLogic();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CallText();
        }
    }

    private void TextLogic()
    {
        if (_letterNumber < _toCopyText.Length)
        {
            if (_timeBetweenLettersValue < _timeBetweenLettersThreshold)
            {
                _timeBetweenLettersValue += Time.deltaTime;
            }
            else
            {
                _timeBetweenLettersValue = 0f;
                _letterNumber++;
                string toInsert = _toCopyText[_letterNumber - 1].ToString();
                _currentTxt += toInsert;
                _txtUi.text = _currentTxt;
                //Debug.Log(_letterNumber - 1 + " " + _toCopyText.Length);
                if (_letterNumber == _toCopyText.Length)
                {
                    StartCoroutine(Next());
                }
            }
        }
    }

    private IEnumerator Next()
    {
        yield return new WaitForSeconds(_timeBetweenLines);
        _letterNumber = 0;
        _lineNumber++;
        if (_dialogs[_txtNumber].Lists.Count > _lineNumber)
        {
            _toCopyText = _dialogs[_txtNumber].Lists[_lineNumber];
        }
        else
        {
            _toCopyText = "";
            _currentTxt = "";
            _go = false;
        }
        _txtUi.text = "";
        _currentTxt = "";
    }

    private void CallText()
    {
        _go = true;
        _letterNumber = 0;
        _lineNumber = 0;
        _txtNumber++;
        _toCopyText = _dialogs[_txtNumber].Lists[_lineNumber];
        _txtUi.text = "";
    }

    private void Initiate()
    {
        _toCopyText = _txts[0];
        int listNb = 0;
        int array = 0;
        foreach (string txt in _txts)
        {
            if (_dialogs.Count < listNb + 1)
            {
                _dialogs.Add(new Sdialog(new List<string>()));
            }
            _dialogs[listNb].Lists.Add(txt);
            array++;
            if (array == _dialogLengths[listNb])
            {
                listNb++;
                array = 0;
            }
        }
    }
}

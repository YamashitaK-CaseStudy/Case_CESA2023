using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBehavior : MonoBehaviour
{
    /*�C���^�[�t�F�C�X�i���J�����o�j*/
    public void AddScore(int value)
    {
        if (value == 0)
        {
            return;
        }

        if (_score >= _MAX_VALUE)
        {
            return;
        }

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            _oldScore = _displayedValue;
        }

        _oldScore = _score;
        _score += value;

        if (_score >= _MAX_VALUE)
        {
            _score = _MAX_VALUE;
        }
        _countTime = 0;

        _coroutine = StartCoroutine(UpdateText());
    }

    /*����J���J�����o*/
    private void Start()
    {
        if (_reachTime <= 0)
        {
            _reachTime = float.Epsilon;
        }

        _textMeshProU = gameObject.GetComponent<TextMeshProUGUI>();

        if (_textMeshProU == null)
        {
            Debug.LogError("TextMeshPro��������܂���ł��� " + gameObject.name + "/ScoreBehavior");
            return;
        }

        _textMeshProU.text = "000000";
    }

    private IEnumerator UpdateText()
    {
        while (_countTime < _reachTime)
        {
            _countTime += Time.deltaTime;

            if (_countTime >= _reachTime)
            {
                _countTime = _reachTime;
            }

            _displayedValue = (int)((_score - _oldScore) * (_countTime / _reachTime) + _oldScore);
            _textMeshProU.text = _displayedValue.ToString();

            int overDigit = _textMeshProU.text.Length - 1;

            _textMeshProU.text = "00000" + _textMeshProU.text;//0��O�ɂT�ǉ��@_textMeshProU.text���O�`�X�̏ꍇ�ɍő包�̂U���ɂȂ�
            _textMeshProU.text.Substring(overDigit);

            yield return null;
        }
    }

    private void OnValidate()
    {
        if (_reachTime <= 0)
        {
            _reachTime = float.Epsilon;
        }
    }

    /*�C���X�y�N�^�Œl��ݒ�*/
    [SerializeField, Header("�X�R�A�̉��Z�J�n����I���܂ł̎���")]
    private float _reachTime;

    /*start()�Œl��ݒ�*/
    private TextMeshProUGUI _textMeshProU;

    /*�����l��ݒ�*/
    private int _score = 0;
    private int _oldScore = 0;
    private int _displayedValue = 0;
    private float _countTime = 0;
    private Coroutine _coroutine = null;

    private const int _MAX_DIGIT = 6;
    private const int _MAX_VALUE = 999999;
}

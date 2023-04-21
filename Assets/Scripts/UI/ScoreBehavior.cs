using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBehavior : MonoBehaviour
{
    /*インターフェイス（公開メンバ）*/
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

    /*非公開公開メンバ*/
    private void Start()
    {
        if (_reachTime <= 0)
        {
            _reachTime = float.Epsilon;
        }

        _textMeshProU = gameObject.GetComponent<TextMeshProUGUI>();

        if (_textMeshProU == null)
        {
            Debug.LogError("TextMeshProが見つかりませんでした " + gameObject.name + "/ScoreBehavior");
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

            _textMeshProU.text = "00000" + _textMeshProU.text;//0を前に５つ追加　_textMeshProU.textが０～９の場合に最大桁の６桁になる
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

    /*インスペクタで値を設定*/
    [SerializeField, Header("スコアの加算開始から終了までの時間")]
    private float _reachTime;

    /*start()で値を設定*/
    private TextMeshProUGUI _textMeshProU;

    /*初期値を設定*/
    private int _score = 0;
    private int _oldScore = 0;
    private int _displayedValue = 0;
    private float _countTime = 0;
    private Coroutine _coroutine = null;

    private const int _MAX_DIGIT = 6;
    private const int _MAX_VALUE = 999999;
}

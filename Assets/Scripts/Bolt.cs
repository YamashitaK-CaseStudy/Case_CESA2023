using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : RotatableObject
{
    [SerializeField, Header("棒状部分の長さ")] private uint _threadLength = 1;
    [SerializeField, Header("移動量の限界")] private uint _translationLimit = 1;
    [SerializeField, Header("回転時の移動量。正で抜ける方向、負で締まる方向。")] private float _amountTranslation = 1.0f;
    [SerializeField, Header("高速回転時の移動スピード")] private float _spinningTranslationSpeed = 1.0f;
    private float _countTranslation = 0;
    private float _countSpinTime = 0;

    private void Start()
    {
        // まわす大の設定
        StartSettingSpin();
    }
    private void Update()
    {
        if (_isRotating || _isSpin)
        {
            if (Mathf.Abs(_countTranslation) >= _translationLimit)
            {
                return;
            }

            if (_isRotating)
            {
                UpdateRotate();
                UpdatePositionInRotation();
            }
            else
            {
                UpdateSpin();
                UpdatePositionInSpin();
            }

        }
    }

    private void UpdatePositionInRotation()
    {
        // 座標更新
        float progressRate = ProgressRate;
        if (progressRate >= 1)//現在は１を越える値が返るようになっている。2023/4/6
        {
            progressRate = 0;
            _countTranslation += _amountTranslation;
        }
        Vector3 localPosition = Vector3.zero;
        localPosition.y = _countTranslation + _amountTranslation * progressRate;
        transform.GetChild(0).localPosition = localPosition;
    }

    private void UpdatePositionInSpin()
    {
        _countSpinTime += Time.deltaTime;
        Vector3 localPosition = Vector3.zero;
        localPosition.y = _spinningTranslationSpeed * _countSpinTime;

        if (Mathf.Abs(localPosition.y) >= _translationLimit)
        {
            _isSpin = false;
            localPosition.y = _translationLimit;
            _countSpinTime = 0;
        }
        transform.GetChild(0).localPosition = localPosition;
    }

    private void OnValidate()
    {
        if (_threadLength == 0)
        {
            _threadLength = 1;
        }
        var threadTransform = transform.GetChild(0).GetChild(1);
        float y_localPos = (_threadLength - 1) * -0.5f;
        float y_localScale = _threadLength * -0.5f;//シリンダーメッシュはスケール１で２の長さがある
        threadTransform.localPosition = new Vector3(0, y_localPos, 0);
        threadTransform.localScale = new Vector3(threadTransform.localScale.x, y_localScale, threadTransform.localScale.z);
    }
}

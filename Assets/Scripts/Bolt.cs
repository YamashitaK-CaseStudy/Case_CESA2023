using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : RotatableObject
{
    [SerializeField, Header("ネジ部のモデル")] private GameObject _threadObject;//インスペクタで設定
    [SerializeField, Header("ボルトの長さ")] private uint _length = 1;
    [SerializeField, Header("最大移動量")] private uint _translationLimit = 1;
    [SerializeField, Header("回転時の移動量。正で抜ける方向、負で締まる方向。")] private float _amountTranslation = 1.0f;
    [SerializeField, Header("高速回転時の移動スピード")] private float _spinningTranslationSpeed = 1.0f;
    [SerializeField, Header("連動するオブジェクトのリスト")] private List<GameObject> _interlockingObjectList;
    private float _countTranslation = 0;
    private float _countSpinTime = 0;
    private List<Vector3> _initialPositionListOfInterlockingObjects = new List<Vector3>();//連動オブジェクトの初期位置
    private Transform _childTransform;

    private void Start()
    {
        // まわす大の設定
        StartSettingSpin();

        foreach (var obj in _interlockingObjectList)
        {
            if (obj == null)
            {
                _interlockingObjectList.RemoveAt(_initialPositionListOfInterlockingObjects.Count);
                continue;
            }
            _initialPositionListOfInterlockingObjects.Add(obj.transform.position);
        }

        _childTransform = transform.GetChild(0);
    }
    private void Update()
    {
        if (_isRotating || _isSpin)
        {
            if (Mathf.Abs(_countTranslation) >= _translationLimit)
            {
                _isRotating = _isSpin = false;
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

            //連動オブジェクトの位置を更新
            int count = _interlockingObjectList.Count;
            for (int i = 0; i < count; ++i)
            {
                _interlockingObjectList[i].transform.position = _initialPositionListOfInterlockingObjects[i] + _childTransform.localPosition;
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
        _childTransform.localPosition = localPosition;
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
        _childTransform.localPosition = localPosition;
    }

    private void OnValidate()
    {
        if (_length == 0)
        {
            _length = 1;
        }

        UnityEditor.EditorApplication.delayCall += () => UpdateLength();
    }

    private void UpdateLength()
    {
        if (this == null)
        {
            return;
        }
        int childCount = transform.GetChild(0).childCount;

        for (int i = 1; i < childCount; ++i)
        {
            DestroyImmediate(transform.GetChild(0).GetChild(childCount - i).gameObject);
        }

        for (int i = 1; i < _length; ++i)
        {
            var threadTransform = Instantiate(_threadObject, transform.GetChild(0)).transform;

            threadTransform.name = "Thread";
            threadTransform.localPosition = new Vector3(0, -i, 0);
        }

    }
}

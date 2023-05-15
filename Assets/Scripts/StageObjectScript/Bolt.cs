using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : RotatableObject
{
    [SerializeField, Header("ネジ部のモデル")] private GameObject _threadObject;//インスペクタで設定
    [SerializeField, Header("ボルトの長さ")] private uint _length = 1;
    [SerializeField, Header("最大移動量")] private uint _translationLimit = 1;
    [SerializeField, Header("回転時の移動量。正で抜ける方向、負で締まる方向。")] private float _TranslationPerRotation = 1.0f;
    [SerializeField, Header("高速回転時の移動スピード")] private float _spinningTranslationSpeed = 1.0f;
    [SerializeField, Header("連動オブジェクトのリスト(親子関係のないオブジェクト)")] private List<GameObject> _interlockingObjectList;
    private float _countTranslation = 0;
    private float _countSpinTime = 0;
    private List<Vector3> _initialPositionListOfInterlockingObjects = new List<Vector3>();//連動オブジェクトの初期位置
    private Transform _childTransform;
    private delegate void FuncUpdateBolt();
    private FuncUpdateBolt UpdateBolt;
    private Transform _rootTransform = null;
    private bool _wasCalculateRootSpaceVec = false;
    private Vector3 _upVectorWorldSpace = Vector3.zero;
    private Vector3 _rootOldPosition = Vector3.zero;

    private void Start()
    {
      
        if (transform.parent == null)
        {
            UpdateBolt = UpdateWhenRoot;
        }
        else
        {
            _rootTransform = transform.root;
            UpdateBolt = UpdateWhenChild;
        }


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
        UpdateBolt();
    }

    private void UpdateWhenRoot()
    {
        if (_isRotating || _isSpining)
        {
            if (Mathf.Abs(_countTranslation) >= _translationLimit)
            {
                _isRotating = _isSpining = false;
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

    private void UpdateWhenChild()
    {
        if (_isRotating == false && _isSpining == false)
        {
            return;
        }

        if (Mathf.Abs(_countTranslation) >= _translationLimit)
        {
            _isRotating = _isSpining = false;
            return;
        }

        if (_isRotating)
        {
            UpdateRotate();
            UpdateRootPositionInRotation();
        }
        else
        {
            UpdateSpin();
            UpdateRootPositionInSpin();
        }

        //連動オブジェクトの位置を更新
        int count = _interlockingObjectList.Count;
        for (int i = 0; i < count; ++i)
        {
            _interlockingObjectList[i].transform.position = _initialPositionListOfInterlockingObjects[i] + _childTransform.localPosition;
        }

    }

    private void UpdatePositionInRotation()
    {
        // 座標更新
        float progressRate = ProgressRate;
        if (progressRate >= 1)
        {
            progressRate = 0;//現在は１を越える値が返るようになっている。2023/4/6
            _countTranslation += _TranslationPerRotation;
        }
        Vector3 localPosition = Vector3.zero;
        localPosition.y = _countTranslation + _TranslationPerRotation * progressRate;
        _childTransform.localPosition = localPosition;
    }

    private void UpdatePositionInSpin()
    {
        _countSpinTime += Time.deltaTime;
        Vector3 localPosition = Vector3.zero;
        localPosition.y = _spinningTranslationSpeed * _countSpinTime;

        if (Mathf.Abs(localPosition.y) >= _translationLimit)
        {
            _isSpining = false;
            localPosition.y = _translationLimit;
            _countSpinTime = 0;
        }
        _childTransform.localPosition = localPosition;
    }

    private void UpdateRootPositionInRotation()
    {
        /*回転始めに一度計算する*/
        if (!_wasCalculateRootSpaceVec)
        {
            var matrix = transform.localToWorldMatrix;
            _upVectorWorldSpace = matrix.GetColumn(1);
            _upVectorWorldSpace = _upVectorWorldSpace.normalized;
            _rootOldPosition = _rootTransform.position;
            _wasCalculateRootSpaceVec = true;
        }

        float progressRate = ProgressRate;

        if (progressRate >= 1)
        {
            _wasCalculateRootSpaceVec = false;
            progressRate = 1;//現在は１を越える値が返るようになっている。2023/4/6
            _countTranslation += _TranslationPerRotation;
        }

        _rootTransform.position = _rootOldPosition + _upVectorWorldSpace * _TranslationPerRotation * progressRate;
    }

    private void UpdateRootPositionInSpin()
    {
        /*回転始めに一度計算する*/
        if (!_wasCalculateRootSpaceVec)
        {
            var matrix = transform.localToWorldMatrix;
            _upVectorWorldSpace = matrix.GetColumn(1);
            _upVectorWorldSpace = _upVectorWorldSpace.normalized;
            _rootOldPosition = _rootTransform.position;
            _wasCalculateRootSpaceVec = true;
        }

        _countSpinTime += Time.deltaTime;

        float translated = (_rootTransform.position - _rootOldPosition).magnitude;

        if (Mathf.Abs(translated) >= _translationLimit)
        {
            _isSpining = false;
            _wasCalculateRootSpaceVec = false;

            _countSpinTime = 0;
            _rootTransform.position = _rootOldPosition + _upVectorWorldSpace * _translationLimit;
            return;
        }

        _rootTransform.position = _rootOldPosition + _upVectorWorldSpace * _spinningTranslationSpeed * _countSpinTime;
    }

    private void OnValidate()
    {
        if (_length == 0)
        {
            _length = 1;
        }

        //UnityEditor.EditorApplication.delayCall += () => UpdateLength();
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

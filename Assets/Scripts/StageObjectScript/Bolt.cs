using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SuzumuraTomoki;

public class Bolt : RotatableObject
{
    private void FixedUpdate()
    {
        //定義することで親クラスのFixedUpdateを止める
    }

    /*定数*/
    public enum UpVectorInWorld
    {
        VERTICAL,
        HORIZONTAL,
    }


    /*変数　非公開*/

    [SerializeField] private GameObject _threadObject;//インスペクタで設定
    [SerializeField] private uint _length = 1;
    [SerializeField] private uint _translationLimit = 1;
    [SerializeField] private float _translationPerRotation = 1.0f;
    /*親オブジェクトで廃止された[SerializeField]*/
    private float _spinningTranslationSpeed = 1.0f;
    [SerializeField, Header("親子関係のないオブジェクトのみ追加できます")] private List<GameObject> _interlockingObjectList;
    private float _countTranslation = 0;
    private float _countSpinTime = 0;
    private float _translationVecPerRotation = 0;
    private List<Vector3> _initialPositionListOfInterlockingObjects = new List<Vector3>();//連動オブジェクトの初期位置
    private Transform _childTransform;
    private delegate IEnumerator FuncUpdateBolt();
    private FuncUpdateBolt UpdateBolt;
    //private Transform _rootTransform = null;
    private bool _wasCalculateRootSpaceVec = false;
    private Vector3 _upVectorWorldSpace = Vector3.zero;
    private Vector3 _rootOldPosition = Vector3.zero;

    /*publics*/
    public uint length
    {
        set
        {
            _length = value;
            ApplyLength();
        }
        get
        {
            return _length;
        }
    }

    public uint translationLimit
    {
        set
        {
            _translationLimit = value;
        }
        get
        {
            return _translationLimit;
        }
    }

    public uint translationPerRotation
    {
        set
        {
            _translationPerRotation = value;
        }
    }
    public uint spinningTranslationSpeed
    {
        set
        {
            _spinningTranslationSpeed = value;
        }
    }

    public UpVectorInWorld upVectorInWorld
    {
        get
        {
            float yVecInW_y = transform.localToWorldMatrix.GetColumn(1).y;
            if (Math.Abs(yVecInW_y) < 0.01f)
            {
                return UpVectorInWorld.HORIZONTAL;
            }
            return UpVectorInWorld.VERTICAL;
        }
    }

    public override void StickRotate(Vector3 center, Vector3 rotAxis, int angle, Transform playerTransform)
    {
        if (_isRotating)
        {
            return;
        }

        //rotAxisがローカルYベクトルと平行じゃなければ終了
        if (!CheckCanHold(rotAxis))
        {
            return;
        }

        bool success = CheckInputStick(rotAxis);//抜ける専用//抜け締め：UpdateTranslationVector(rotAxis);

        if (!success)
        {
            return;
        }

        //リミットチェック
        if (_countTranslation >= _translationLimit)
        {
            if (_translationVecPerRotation > 0)
            {
                return;
            }
        }
        else if (_countTranslation <= 0)
        {
            if (_translationVecPerRotation < 0)
            {
                return;
            }
        }

        /*全てのチェックを通過*/

        if (angle < 0)
        {
            offsetRotAxis = new Vector3(0, -1, 0);
        }
        else
        {
            offsetRotAxis = new Vector3(0, 1, 0);
        }

        StartRotate(center, rotAxis, 90, playerTransform);
        PlayPartical();
    }

    public void AddInterlockingObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        _interlockingObjectList.Add(obj);

        ManageInterlockingObjects();
    }

    public void ApplyInspector()
    {
        if (_translationPerRotation < 0)
        {
            _translationPerRotation = 0;
        }
        if (_rotRequirdTime <= 0)
        {
            _rotRequirdTime = float.Epsilon;
        }
        ApplyLength();
        ManageInterlockingObjects();
    }

    public bool CheckCanHold(Vector3 rotAxis)
    {
        Vector3 vec3 = transform.localToWorldMatrix.GetColumn(1);
        var dot = Vector3.Dot(vec3.normalized, rotAxis.normalized);

        if (Math.Abs(dot) > 0.9f)
        {
            return true;
        }

        return false;
    }


    /*privates*/

    private void Start()
    {
        _translationVecPerRotation = _translationPerRotation;

        if (transform.parent == null)
        {
            UpdateBolt = UpdateWhenRoot;
        }
        else
        {
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
        _observer = GameObject.FindWithTag("Observer");
    }

    private IEnumerator UpdateWhenRoot()
    {
        while (_isRotating)
        {
            if (Math.Abs(_countTranslation) >= _translationLimit)
            {
                _isRotating = false;
                break;
            }

            UpdateRotate();
            UpdatePositionInRotationWhenRoot();

            //連動オブジェクトの位置を更新
            int count = _interlockingObjectList.Count;
            for (int i = 0; i < count; ++i)
            {
                _interlockingObjectList[i].transform.position = _initialPositionListOfInterlockingObjects[i] + _childTransform.localPosition;
            }

            yield return null;
        }
    }

    private IEnumerator UpdateWhenChild()
    {
        while (_isRotating)
        {



            UpdateRotate();
            UpdatePositionInRotationWhenChild();

            //連動オブジェクトの位置を更新
            int count = _interlockingObjectList.Count;
            for (int i = 0; i < count; ++i)
            {
                _interlockingObjectList[i].transform.position = _initialPositionListOfInterlockingObjects[i] + _childTransform.localPosition;
            }

            yield return null;
        }

    }

    private void UpdatePositionInRotationWhenRoot()
    {
        // 座標更新
        float progressRate = ProgressRate;
        if (progressRate >= 1)
        {
            _isRotating = false;
            progressRate = 0;//現在は１を越える値が返るようになっている。2023/4/6
            _countTranslation += _translationVecPerRotation;
        }
        Vector3 localPosition = Vector3.zero;
        localPosition.y = _countTranslation + _translationVecPerRotation * progressRate;
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

    private void UpdatePositionInRotationWhenChild()
    {
        /*回転始めに一度計算する*/
        if (!_wasCalculateRootSpaceVec)
        {
            var matrix = transform.localToWorldMatrix;
            _upVectorWorldSpace = matrix.GetColumn(1);
            _upVectorWorldSpace = _upVectorWorldSpace.normalized;
            _rootOldPosition = transform.root.position;
            _wasCalculateRootSpaceVec = true;
        }

        float progressRate = ProgressRate;

        if (progressRate >= 1)
        {
            _isRotating = false;
            _wasCalculateRootSpaceVec = false;
            progressRate = 1;//現在は１を越える値が返るようになっている。2023/4/6
            _countTranslation += _translationVecPerRotation;
        }

        transform.root.position = _rootOldPosition + _upVectorWorldSpace * _translationVecPerRotation * progressRate;
    }

    private void UpdateRootPositionInSpin()
    {
        /*回転始めに一度計算する*/
        if (!_wasCalculateRootSpaceVec)
        {
            var matrix = transform.localToWorldMatrix;
            _upVectorWorldSpace = matrix.GetColumn(1);
            _upVectorWorldSpace = _upVectorWorldSpace.normalized;
            _rootOldPosition = transform.root.position;
            _wasCalculateRootSpaceVec = true;
        }

        _countSpinTime += Time.deltaTime;

        float translated = (transform.root.position - _rootOldPosition).magnitude;

        if (Mathf.Abs(translated) >= _translationLimit)
        {
            _isSpining = false;
            _wasCalculateRootSpaceVec = false;

            _countSpinTime = 0;
            transform.root.position = _rootOldPosition + _upVectorWorldSpace * _translationLimit;
            return;
        }

        transform.root.position = _rootOldPosition + _upVectorWorldSpace * _spinningTranslationSpeed * _countSpinTime;
    }

    private void ApplyLength()
    {
        if (_length == 0)
        {
            _length = 1;
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

    private void ManageInterlockingObjects()
    {
        //foreachは要素数が変わる処理に弱い
        int listCount = _interlockingObjectList.Count;
        for (int i = 0; i < listCount; ++i)
        {
            var iObj = _interlockingObjectList[i];

            if (iObj == null)
            {
                continue;
            }

            if (transform.root.gameObject.Equals(iObj.transform.root.gameObject))
            {
                _interlockingObjectList.Remove(iObj);
                --listCount;
                --i;
            }
        }
    }

    private new void StartRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle, Transform playerTransform)
    {
        _isRotating = true;

        GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.Rotate);

        _playerTransform = playerTransform;

        // 回転の中心を設定
        _axisCenterWorldPos = rotCenter;
        // 回転軸を設定
        _rotAxis = rotAxis;
        // 回転オフセット値をセット
        _angle = rotAngle;
        // フラグを立てる
        _isRotating = true;
        _isUnion = false;

        // 角度による補正値を計算する
        _polatAngle = _angle / 90;

        // 回転前の角度を持っておく
        _oldRotAngle = transform.rotation;
        _oldPos = transform.position;

        // 床と連鎖の当たり判定を行う
        SetChildHitCheckFloorFlg(true);
        SetChildHitCheckChainFlg(true);
        // めり込み判定を切っておく
        SetChildCheckIntoFloor(false);
        SetChildCheckIntoChain(false);

        // 経過時間を初期化
        _elapsedTime = 0.0f;
        // トレイルの起動
        PlayPartical();

        StartCoroutine(UpdateBolt());
    }

    private new void UpdateRotate()
    {
        if (_doOnce)
        {
            _isRotateStartFream = false;
        }

        if (!_doOnce)
        {
            _isRotateStartFream = true;
            _doOnce = true;
        }

        //デルタタイムを1回転に必要な時間で割った値
        float requiredDeltaTime = Time.deltaTime / (_rotRequirdTime * Math.Abs(_polatAngle));
        _elapsedTime += requiredDeltaTime;

        bool isFinish = false;
        if (_elapsedTime >= 1)
        {   // 修了確認
            requiredDeltaTime -= (_elapsedTime - 1); // 補正
            isFinish = true;
        }   // 90度進むごとに確認当たってるかどうかを確認する
        else if (_elapsedTime >= 1 / Math.Abs(_polatAngle))
        {
            SetChildHitCheckFloorFlg(true);
            if (_isHitFloor)
            {
                requiredDeltaTime -= (_elapsedTime - 1); // 補正
                isFinish = true;
            }
        }

        // 途中で磁石オブジェクトに当たっていた場合の処理
        if (_isUnion && !_isHitFloor)
        {
            isFinish = true;
            requiredDeltaTime -= (_elapsedTime - 1); // 補正
        }

        // 現在フレームの回転を示す回転のクォータニオン作成
        var angleAxis = Quaternion.AngleAxis(_angle * requiredDeltaTime, _rotAxis);

        // 円運動の位置計算
        var tr = transform;

        // 向き更新
        tr.rotation = angleAxis * tr.rotation;

        _oldAngle = _elapsedTime * _angle;


        if (!isFinish) return;

        /***** 終了時処理*****/

        // 終了時に変更するフラグを変更
        _isRotating = false;        // 回転処理の終了
        _isRotateEndFream = true;   // 回転が終わったFを通知

        // 最終的に回転した量
        var finishAngle = _angle;

        // 床に当たってた時の処理
        if (_isHitFloor)
        {
            // 経過時間と補間用数値を用いて現在進んだ角度から一番近い90単位の角度を算出
            finishAngle = (int)Math.Round(_elapsedTime * _polatAngle, 0, System.MidpointRounding.AwayFromZero) * 90;
            SetReflect(_axisCenterWorldPos, _rotAxis, finishAngle);
        }

        if (_isUnion)
        {
            _isUnion = false;
            finishAngle = (int)Math.Round(_elapsedTime * _polatAngle, 0, System.MidpointRounding.AwayFromZero) * 90;
        }

        // 最終的に回転した量を考慮して最終補正をクオータニオンで計算する
        // 現在フレームの回転を示す回転のクォータニオン作成
        var qtAngleAxis = Quaternion.AngleAxis(finishAngle, _rotAxis);
        // 円運動の位置計算
        // クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
        // _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
        this.transform.rotation = qtAngleAxis * _oldRotAngle;

        // プレイヤー起因の回転かを判定
        if (_playerTransform != null)
        {
            var playerComp = _playerTransform.GetComponent<Player>();
            // プレイヤーに回転終了通知を飛ばす
            playerComp.NotificationEndRotate();
            // バグ防止
            _playerTransform = null;
        }
        StopPartical();
        if (_isRotateEndFream)
        {
            // 普段は当たり判定の処理を切っておく
            SetChildHitCheckFloorFlg(false);
            SetChildHitCheckChainFlg(false);
            // めり込み判定の確認
            SetChildCheckIntoFloor(true);
            SetChildCheckIntoChain(true);
        }

        if (_isHitFloor)
        {
            SetReflect(_axisCenterWorldPos, _rotAxis, _angle);
        }

        _doOnce = false;
        _isRotateEndFream = false;
    }

    private bool UpdateTranslationVector(Vector3 rotAxis)
    {
        var stick_xy = SceneManager.playerInput.FindAction("RotaionSelect").ReadValue<Vector2>();

        if (rotAxis == Vector3.up)
        {
            if (stick_xy.x > 0.1f)
            {
                _translationVecPerRotation = _translationPerRotation;
                return true;
            }
            else if (stick_xy.x < -0.1f)
            {
                _translationVecPerRotation = _translationPerRotation * -1;
                return true;
            }
        }
        else if (rotAxis == Vector3.right)
        {
            if (stick_xy.y > 0.1f)
            {
                _translationVecPerRotation = _translationPerRotation;
                return true;
            }
            else if (stick_xy.y < -0.1f)
            {
                _translationVecPerRotation = _translationPerRotation * -1;
                return true;
            }
        }
        else
        {
            Debug.LogError("Bolt 予期しない動作");
        }

        return false;
    }

    private bool CheckInputStick(Vector3 rotAxis)
    {
        var stick_xy = SceneManager.playerInput.FindAction("RotaionSelect").ReadValue<Vector2>();

        if (rotAxis == Vector3.up)
        {
            if (stick_xy.y > 0.2f && Math.Abs(stick_xy.x) < stick_xy.y / 2.0f)
            {
                return true;
            }
        }
        else if (rotAxis == Vector3.right)
        {
            //向きが左の時と右の時がある
            var localY_inWorld = transform.localToWorldMatrix.GetColumn(1);
            float absRatioY = Math.Abs(stick_xy.x) / 2.0f;
            if (localY_inWorld.x > 0)
            {
                if (stick_xy.x > 0.2f && Math.Abs(stick_xy.y) < absRatioY)
                {
                    return true;
                }
            }
            else
            {
                if (stick_xy.x < -0.2f && Math.Abs(stick_xy.y) < absRatioY)
                {
                    return true;
                }
            }
        }
        else
        {
            Debug.LogError("Bolt 予期しない動作");
        }

        return false;
    }
}


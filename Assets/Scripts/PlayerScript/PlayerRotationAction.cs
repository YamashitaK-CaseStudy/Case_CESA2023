using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour{

    private bool _isRotating = true;

    private RightStickRotAngle _stricRotAngle = null;
    private InputAction _blocklockButton = null;
    private InputAction _rotationSpinButton = null;

    // ロックしている時のオブジェクト
    private GameObject _lockRootObject = null;
    private GameObject _lockObjectParts = null;
    private bool _isLock = false;
    private BlockPriorty _blockPriorty;

    // 回転オブジェクトのコンポーネント
    private RotatableObject _rotComp = null;
    private RotObjkinds _rotatbleKind = null;

    // ブロック優先
    enum BlockPriorty {
        Bottom,Front,None
    }

    private void PlayerRotationStart() {

        // 右スティックコンポネント取得
        _stricRotAngle = GetComponent<RightStickRotAngle>();

        // 各種ボタンの取得
        _blocklockButton    = _playerInput.actions.FindAction("BlockLock");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");

        // プレイヤーのロックフレームの色変更
        LockFreamChangeColor(Color.red);
    }

    private void PlayerRotationUpdate() {

        // ロックフレームの可視化
        LockFreamDisplay(_bottomHitCheck, _frontHitCheck);

        // ロックボタンを押された時
        if (_blocklockButton.IsPressed()) {
            if (!_isLock) {
                Debug.Log("ブロックロック");
                _isLock = true;

                // ブロックの優先度確認
                BlockPriority(_bottomHitCheck, _frontHitCheck);

                // 各アニメーションの遷移
                AnimatoinState(true);

                // ブロック全部のフレーム可視化
                LockFreamMassSetActive(_lockRootObject, true);

                // 頭上の当たり判定rayを伸ばす(ボルト用に)
                _upperrayCheck.SetDistacne(1.5f);
            }
        }

        // ロックボタンが離された時
        if (_blocklockButton.WasReleasedThisFrame()) {
            Debug.Log("ブロックロック解除");
            _isLock = false;
            LockFreamMassSetActive(_lockRootObject, false);
            _stricRotAngle.yAxisDestroyDamiobj();
            AnimatoinState(false);
            _upperrayCheck.SetDistacne(0.7f);
        }

        if ((_isLock && _animCallBack.GetIsRotationValid)) {

            // 下の回転オブジェクトを参照
            if (_blockPriorty == BlockPriorty.Bottom) {

                // ブロックごとに回転の種類が違うのでKindを使って仕分ける
                // 通常ブロック操作
                if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject ||
                    _rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {
                    Debug.Log("通常回転操作");

                    if (_rotComp._isRotateEndFream) {
                        Debug.Log("回転終了");
                        _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
                    }

                    _stricRotAngle.StickRotY_Update();
                    _rotComp.StickRotateY(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

                    if (_stricRotAngle._isOldAngleChinge) {

                        _animator.SetFloat("RotationSpeed", 6);
                        if(_rotComp.offsetRotAxis.y == 1) {
                            _animator.SetTrigger("Rotation_Y_Right");
                        }
                        else if(_rotComp.offsetRotAxis.y == -1) {
                            _animator.SetTrigger("Rotation_Y_Left");
                        }
                    }
                }

                // ボルト操作
                else if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {

                    Debug.Log("ボルト回転操作");
                    _animator.SetFloat("RotationSpeed", 1);
                    if (!_upperrayCheck.IsUpperHit) {
                        _rotComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

                        if (_rotComp._isRotateStartFream) {
                            _animator.SetTrigger("Rotation_Y_Right");
                            Debug.Log("ボルト回転はじめ");
                        }
                    }
                }

                // 高速回転操作
                else if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {
                    Debug.Log("高速回転操作");

                    if (_rotationSpinButton.WasPressedThisFrame()) {

                        if (_rotComp._isSpining) {
                            Debug.Log("高速回転終了");
                            _rotComp.EndSpin();
                        }
                        else {
                            _rotComp.StartSpin(CompensateRotationAxis(_lockObjectParts.transform.position), Vector3.up);
                        }
                    }
                }
            }
            else if (_blockPriorty == BlockPriorty.Front) {

                // ブロックごとに回転の種類が違うのでKindを使って仕分ける
                // 通常ブロック操作
                if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject ||
                    _rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {

                    _stricRotAngle.StickRotX_Update();
                    _rotComp.StickRotateY(CompensateRotationAxis(_frontHitCheck.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);
                }

                // ボルト操作
                else if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {

                    if (!_backHItCheck.GetIsHit && _lockObjectParts.tag == "ScrewObj") {
                        _rotComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleY, this.transform);
                    }
                }

                // 高速回転操作
                else if (_rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {

                    if (_rotationSpinButton.WasPressedThisFrame()) {

                        if (_rotComp._isSpining) {
                            Debug.Log("高速回転終了");
                            _rotComp.EndSpin();
                        }
                        else {
                            _rotComp.StartSpin(CompensateRotationAxis(_lockObjectParts.transform.position), Vector3.right);
                        }
                    }
                }
            }
        }
    }

    // ロック可能な回転オブジェクトを可視化する
    private void LockFreamDisplay(RotObjHitCheck _bottom, RotObjHitCheck _front) {

        if (_bottom.GetRotPartsObj != null) {

            Debug.Log("可能判定下");
            _LockFreamObj.transform.position = _bottom.GetRotPartsObj.transform.position;
            _LockFreamObj.active = true;

            // ボルトの向きが横向きであれば可視化しない
            var boltComp = _bottom.GetRotObj.GetComponent<Bolt>();
            if (boltComp != null && boltComp.upVectorInWorld == Bolt.UpVectorInWorld.HORIZONTAL) {
                _LockFreamObj.active = false;
            }
        }
        else if (_front.GetRotPartsObj != null) {

            Debug.Log("可能判定左右");
            _LockFreamObj.transform.position = _front.GetRotPartsObj.transform.position;
            _LockFreamObj.active = true;

            // ボルトの向きが盾向きであれば可視化しない
            var boltComp = _front.GetRotObj.GetComponent<Bolt>();
            if (boltComp != null && boltComp.upVectorInWorld == Bolt.UpVectorInWorld.VERTICAL || 
                _front.GetRotPartsObj.tag == "ThreadObj") {
                _LockFreamObj.active = false;
            }
        }
        else {

            _LockFreamObj.active = false;
        }
    }

    // 下か左右どっちを参照するか関数
    private void BlockPriority(in RotObjHitCheck _bottom, in RotObjHitCheck _front) {

        // 下にオブジェクトがあるか確認
        if (_bottom.GetRotObj != null) {

            _blockPriorty = BlockPriorty.Bottom;
            _lockRootObject = _bottom.GetRotObj;
            _lockObjectParts = _bottom.GetRotPartsObj;
            _stricRotAngle.yAxisManyObjJude(_bottom);
            GetRotationComponent();
        }
        else if (_front.GetRotObj != null) {

            _blockPriorty = BlockPriorty.Front;
            _lockRootObject = _front.GetRotObj;
            _lockObjectParts = _front.GetRotPartsObj;
            GetRotationComponent();
        }
        else {

            _blockPriorty = BlockPriorty.None;
            _lockRootObject = null;
            _lockObjectParts = null;
        }
    }

    // アニメーションのステート関数
    private void AnimatoinState(bool _isState) {

        switch (_isState) {
            case true:

                // 下に回転オブジェクトがある場合
                if (_bottomHitCheck.GetRotObj != null) {

                    // 頭にブロックがない場合
                    if (!_upperrayCheck.IsUpperHit) {
                        _yBlockLock = true;
                    }
                    // 頭にブロックがある場合
                    else {
                        _yBlockUpperLock = true;
                    }

                    // 座標補正
                    Pos_Correction(_bottomHitCheck.GetRotPartsObj.transform.position);
                }

                // 左右に回転オブジェクトがある場合
                else if(_frontHitCheck.GetRotObj != null) {

                    _xBlockLock = true;

                    // ボルトのねじ部分であれば遷移しない
                    if (_frontHitCheck.GetRotPartsObj.tag == "ThreadObj") {
                        _xBlockLock = false;
                    }
                }

                // 下に回転オブジェクトが無い場合
                else {
                    // 頭にブロックがない場合
                    if (!_upperrayCheck.IsUpperHit) {
                        _yBlockLock = true;
                    }
                }

                break;

            case false:

                _yBlockLock = false;
                _xBlockLock = false;
                _yBlockUpperLock = false;
                _animCallBack.RotationInValid();

                break;
        }
    }

    private void LockFreamMassSetActive(GameObject _object, bool _displayflg) {

        if (_object == null) {
            return;
        }

        // 対象オブジェクトの子オブジェクトをチェックする
        foreach (Transform child in _object.transform) {

            // 子オブジェクトのアクティブを切り替える
            GameObject childObject = child.gameObject;

            if (childObject.tag == "LockFream") {
                childObject.SetActive(_displayflg);
            }

            // 再帰的に全ての子オブジェクトを処理する
            LockFreamMassSetActive(childObject, _displayflg);
        }
    }

    // ロックフレームの色を変更
    private void LockFreamChangeColor(Color color) {
        var mat = _LockFreamObj.GetComponent<MeshRenderer>().material;
        mat.SetColor("_Color", color);
    }

    // プレイヤーの座標補正
    private void Pos_Correction(in Vector3 center) {

        var Pos = transform.position;
        var pPos = new Vector3(center.x, Pos.y, 0);
        transform.transform.position = pPos;
    }

    // 座標補正　Playerのやつマルコピ
    private Vector3 CompensateRotationAxis(in Vector3 AXIS) {
        return new Vector3(RoundOff(AXIS.x), RoundOff(AXIS.y), RoundOff(AXIS.z));
    }

    private int RoundOff(float value) {
        int valueInt = (int)value;

        if (value - valueInt < 0.5f) {
            return valueInt;
        }

        return ++valueInt;
    }

    public void GetRotationComponent() {

        _rotComp = _lockRootObject.GetComponent<RotatableObject>();
        _rotatbleKind = _lockRootObject.GetComponent<RotObjkinds>();
    }

    public void NotificationStartRotate() {
        _isRotating = true;
    }

    public void NotificationEndRotate() {
        _isRotating = false;
    }
}



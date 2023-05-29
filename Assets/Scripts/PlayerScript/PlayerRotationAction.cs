using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour{

    private bool _isRotating = true;

    private RightStickRotAngle _stricRotAngle = null;
   
    private InputAction _blocklockButton = null;
    private InputAction _rotationButton = null;
    private InputAction _rotationSpinButton = null;

    // ロックしている時のオブジェクト
    private GameObject _lockObject = null;
    private GameObject _lockObjectParts = null;
    private bool _isLock = false;
    private BlockPriorty _blockPriorty;

    // ブロック優先
    enum BlockPriorty {
        Bottom,Front,None
    }

    private void PlayerRotationStart() {

        // 右スティックコンポネント取得
        _stricRotAngle = GetComponent<RightStickRotAngle>();

        // 各種ボタンの取得
        _blocklockButton    = _playerInput.actions.FindAction("BlockLock");
        _rotationButton     = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");

        // フレームの色を赤色にする
        LockFreamChangeColor(Color.red);
    }
    static int a = 0;
    private void PlayerRotationUpdate() {

        // ロックフレームの可視化
        LockFreamDisplay(_bottomHitCheck, _frontHitCheck);

        if (_blocklockButton.IsPressed()) {

            // 移動中、ジャンプ中は回転させない
            if (Mathf.Abs(_speedx) > 0 || !_groundCheck.IsGround) {
              
                AnimatoinState(false);
                LockFreamMassSetActive(_lockObject, false);
                _upperrayCheck.SetDistacne(0.7f);
                _isLock = false;

                return;
            }
            else if (!_isLock) {

                // ブロックの優先度確認
                BlockPriority(_bottomHitCheck, _frontHitCheck);

                // アニメーションの遷移
                AnimatoinState(true);

                // ブロック全部のフレーム可視化
                LockFreamMassSetActive(_lockObject,true);

                // 頭上の当たり判定rayを伸ばす(ボルト用に)
                _upperrayCheck.SetDistacne(1.5f);

                _isLock = true;
                Debug.Log("ブロックロック");
            }
        }
        
        // ブロックロック解除
        if (_blocklockButton.WasReleasedThisFrame()) {

            AnimatoinState(false);
            LockFreamMassSetActive(_lockObject,false);
            _isLock = false;
            _upperrayCheck.SetDistacne(0.7f);
//            Debug.Log("ブロックロック解除");
        }
      
        // Lock中
        if ((_isLock && _animCallBack.GetIsRotationValid) ) {

            // どっちにも当たってなければ抜ける
            if (_lockObject == null) {
                Debug.Log("ブロック検知してない");
                return;
            }

            var rotatebleboltComp = _lockObject.GetComponent<Bolt>();
            var rotatbleComp = _lockObject.GetComponent<RotatableObject>();
            var rotatbleKind = _lockObject.GetComponent<RotObjkinds>();

            // 下の回転オブジェクトを参照
            if (_blockPriorty == BlockPriorty.Bottom) {

                // Debug.Log("下オブジェクト取得中");

                // ブロックごとに回転の種類が違うのでKindを使って仕分ける
                // 通常の黄色ブロックと磁石ブロックはスティックでの操作にて行う
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject || rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {

                    // スティックでの回転
                    if (rotatbleComp._isRotateEndFream) {
                        _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
                    }
                    _stricRotAngle.StickRotY_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

                    // ロック時のY回転アニメーション
                    if (_stricRotAngle._isOldAngleChinge) {

                        // 通常回転時のアニメーションスピードの変更
                        _animator.SetFloat("RotationSpeed", 6);

                        if (rotatbleComp.offsetRotAxis.y == 1) {
                            _animator.SetTrigger("Rotation_Y_Right");
                        }
                        else if (rotatbleComp.offsetRotAxis.y == -1) {
                            _animator.SetTrigger("Rotation_Y_Left");
                        }
                    }
                }

                // ボルトブロック操作
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject && !_upperrayCheck.IsUpperHit) {

                    _stricRotAngle.StickRotY_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

                    // ボルトにとってる状態のY回転アニメーション
                    if (rotatbleComp._isRotateStartFream) {
                        _animator.SetFloat("RotationSpeed", 1);
                        _animator.SetTrigger("Rotation_Y_Right");
                    }
                }
              
                // 高速回転はスティックの右左倒した自転で始まる
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {

                    // R3押し込み時の処理
                    if (_rotationSpinButton.WasPressedThisFrame()) {

                        if (rotatbleComp._isSpining) {
                            Debug.Log("高速回転終了");
                            _lockObject = null;
                            rotatbleComp.EndSpin();
                        }
                        else {

                            rotatbleComp.StartSpin(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.up);
                        }
                    }
                }
            }

            // 左右の回転オブジェクトを参照
            else if (_blockPriorty == BlockPriorty.Front) {

                // ブロックごとに回転の種類が違うのでKindを使って仕分ける
                // 通常の黄色ブロックと磁石ブロックはスティックでの操作にて行う
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject || rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {

                    // スティック回転X
                    _stricRotAngle.StickRotX_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);
                }

                // ボルトブロック操作
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {
                    if (_lockObjectParts.tag == "ScrewObj") {
                        if (rotatebleboltComp.upVectorInWorld == Bolt.UpVectorInWorld.HORIZONTAL) {
                            if (!_backHItCheck.GetIsHit) {
                                _stricRotAngle.StickRotY_Update();
                                rotatbleComp.StickRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleY, this.transform);
                            }
                        }
                    }
                }

                // 高速回転はスティックの押し込みにて行う
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {

                    // R3押し込み時の処理
                    if (_rotationSpinButton.WasPressedThisFrame()) {

                        if (rotatbleComp._isSpining) {
                            Debug.Log("高速回転終了");

                            rotatbleComp.EndSpin();
                        }
                        else {

                            rotatbleComp.StartSpin(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right);
                        }
                    }
                }
            }
        }
    }

    private void Pos_Correction(in Vector3 center) {

        var Pos = transform.position;
        var pPos = new Vector3(center.x, Pos.y, 0);
        transform.transform.position = pPos;
    }

    public void NotificationStartRotate() {
        _isRotating = true;
    }

    public void NotificationEndRotate() {
        _isRotating = false;
    }

    // 下か左右どっちを参照するか関数
    private void BlockPriority(in RotObjHitCheck _bottom, in RotObjHitCheck _front) {

        // 下にオブジェクトがあるか確認
        if (_bottom.GetRotObj != null) {

            _blockPriorty = BlockPriorty.Bottom;
            _lockObject = _bottom.GetRotObj;
            _lockObjectParts = _bottom.GetRotPartsObj;
        }
        else if (_front.GetRotObj != null) {
            _blockPriorty = BlockPriorty.Front;
            _lockObject = _front.GetRotObj;
            _lockObjectParts = _front.GetRotPartsObj;
        }
        else {
            _blockPriorty = BlockPriorty.None;
            _lockObject = null;
            _lockObjectParts = null;
        }
    }

    // アニメーションのステート関数
    private void AnimatoinState(bool stateflg) {

        switch (stateflg) {
            case true:

                // 下に回転オブジェクトがありかつ頭の上にブロックがない場合のアニメーション
                if (_bottomHitCheck.GetRotObj != null && !_upperrayCheck.IsUpperHit) {

                    Debug.Log(" 下に回転オブジェクトがありかつ頭の上にブロックがない場合のアニメーション");
                    Pos_Correction(_bottomHitCheck.GetRotPartsObj.transform.position);
                    _yBlockLock = true;
                }
                // 下に回転オブジェクトがありかつ頭の上にブロックがある場合のアニメーション
                else if (_bottomHitCheck.GetRotObj != null && _upperrayCheck.IsUpperHit) {

                    Pos_Correction(_bottomHitCheck.GetRotPartsObj.transform.position);
                    _yBlockUpperLock = true;
                }
                // 左右に回転オブジェクトがあった場合のアニメーション
                else if (_frontHitCheck.GetRotObj != null) {

                    if (_frontHitCheck.GetRotPartsObj.tag == "ScrewObj" && _frontHitCheck.GetRotPartsObj.transform.parent.parent.gameObject.GetComponent<Bolt>().upVectorInWorld == Bolt.UpVectorInWorld.HORIZONTAL) {
                        _xBlockLock = true;
                    }
                    else if (_frontHitCheck.GetRotObj.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.BoltRotObject) {

                        _xBlockLock = true;
                    }
                    else {
                        _yBlockLock = true;
                    }
                }
                // 下に回転オブジェクトが無くかつ頭にブロックがある場合のアニメーション
                else if (_bottomHitCheck.GetRotObj == null && _upperrayCheck.IsUpperHit) {

                }
                // 下に回転オブジェクトが無く頭にブロックがない場合のアニメーション
                else {
                    _yBlockLock = true;
                }

                break;

            case false:

                _yBlockLock = false;
                _xBlockLock = false;
                _yBlockUpperLock = false;
                _animCallBack.RotationInValid();
                _isLock = false;

                break;
        }
    }

    // ロックフレームの色を変更
    private void LockFreamChangeColor(Color color) {
        var mat = _LockFreamObj.GetComponent<MeshRenderer>().material;
        mat.SetColor("_Color", color);
    }

    // ロック可能な時のみ表示
    private void LockFreamDisplay(RotObjHitCheck _bottom,RotObjHitCheck _front) {

        if (_bottom.GetRotPartsObj != null) {
           
            if(_bottom.GetRotPartsObj.transform.parent.parent.gameObject.GetComponent<RotObjkinds>()._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {
                if (_bottom.GetRotPartsObj.tag == "ScrewObj" && _bottom.GetRotPartsObj.transform.parent.parent.gameObject.GetComponent<Bolt>().upVectorInWorld == Bolt.UpVectorInWorld.VERTICAL) {
                    Debug.Log("当たり下" + _bottom.GetRotPartsObj.transform.position);
                    _LockFreamObj.transform.position = _bottom.GetRotPartsObj.transform.position;
                    _LockFreamObj.active = true;
                }
                else {
                    _LockFreamObj.active = false;
                }
            }
            else {
                Debug.Log("当たり下" + _bottom.GetRotPartsObj.transform.position);
                _LockFreamObj.transform.position = _bottom.GetRotPartsObj.transform.position;
                _LockFreamObj.active = true;
            }
        }
        else if(_front.GetRotPartsObj != null) {

            if (_front.GetRotPartsObj.transform.parent.parent.gameObject.GetComponent<RotObjkinds>()._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {
                if (_front.GetRotPartsObj.tag == "ScrewObj" && _front.GetRotPartsObj.transform.parent.parent.gameObject.GetComponent<Bolt>().upVectorInWorld == Bolt.UpVectorInWorld.HORIZONTAL) {
                    Debug.Log("当たり左右" + _front.GetRotPartsObj);
                    _LockFreamObj.transform.position = _front.GetRotPartsObj.transform.position;
                    _LockFreamObj.active = true;
                }
                else {
                    _LockFreamObj.active = false;
                }
            }
            else {
                _LockFreamObj.transform.position = _front.GetRotPartsObj.transform.position;
                _LockFreamObj.active = true;
            }
        }
        else {
            _LockFreamObj.active = false;
        }
    }

    private void LockFreamMassSetActive(GameObject _object, bool _displayflg) {

        if(_object == null) {
            return;
        }

        // 対象オブジェクトの子オブジェクトをチェックする
        foreach (Transform child in _object.transform) {

            // 子オブジェクトのアクティブを切り替える
            GameObject childObject = child.gameObject;

            if(childObject.tag == "LockFream") {
                childObject.SetActive(_displayflg);
            }

            // 再帰的に全ての子オブジェクトを処理する
            LockFreamMassSetActive(childObject, _displayflg);
        }
    }
}



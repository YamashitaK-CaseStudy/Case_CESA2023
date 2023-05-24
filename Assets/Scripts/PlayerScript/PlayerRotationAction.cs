using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour{

    private bool _isRotating = true;

    private StickRotAngle _stricRotAngle = null;
   
    private InputAction _blocklockButton = null;
    private InputAction _rotationButton = null;
    private InputAction _rotationSpinButton = null;

    // ロックしている時のオブジェクト
    private GameObject _lockObject = null;
    private bool _isLock = false;
    private BlockPriorty _blockPriorty;

    // ブロック優先
    enum BlockPriorty {
        Bottom,Front,None
    }

    private void PlayerRotationStart() {

        // 右スティックコンポネント取得
        _stricRotAngle = GetComponent<StickRotAngle>();

        // 各種ボタンの取得
        _blocklockButton    = _playerInput.actions.FindAction("BlockLock");
        _rotationButton     = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");

        // フレームの色を赤色にする
        LockFreamChangeColor(Color.red);
    }

    private void PlayerRotationUpdate() {

        // ロックフレームの可視化
        LockFreamDisplay(_bottomHitCheck, _frontHitCheck);

        // 移動中、ジャンプ中は回転させない
        if (Mathf.Abs(_speedx) > 0 || !_groundCheck.IsGround) {
            return;
        }

        if (_blocklockButton.IsPressed()) {
            if (!_isLock) {

                // ブロックの優先度確認
                _lockObject = BlockPriority(_bottomHitCheck, _frontHitCheck);

                // アニメーションの遷移
                AnimatoinState(true);

                // ブロック全部のフレーム可視化
                LockFreamMassSetActive(_lockObject,true);

                _isLock = true;
                Debug.Log("ブロックロック");
            }
        }
        
        // ブロックロック解除
        if (_blocklockButton.WasReleasedThisFrame()) {

            AnimatoinState(false);
            LockFreamMassSetActive(_lockObject,false);
            _isLock = false;
            Debug.Log("ブロックロック解除");
        }

        // Lock中
        if ((_isLock && _animCallBack.GetIsRotationValid) || _yBlockLock || _yBlockUpperLock || _xBlockLock) {

            // どっちにも当たってなければ抜ける
            if (_lockObject == null) {
                Debug.Log("ブロック検知してない");
                return;
            }

            var rotatbleComp = _lockObject.GetComponent<RotatableObject>();
            var rotatbleKind = _lockObject.GetComponent<RotObjkinds>();

            // 下の回転オブジェクトを参照
            if (_blockPriorty == BlockPriorty.Bottom) {

                Debug.Log("下オブジェクト取得中");

                // ブロックごとに回転の種類が違うのでKindを使って仕分ける
                // 通常の黄色ブロックと磁石ブロックはスティックでの操作にて行う
                if(rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject || rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {

                    // スティックでの回転
                    if (rotatbleComp._isRotateEndFream) {
                        _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
                    }
                    _stricRotAngle.StickRotY_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);

                    if (rotatbleComp._isRotateStartFream && (rotatbleComp.offsetRotAxis.y == 1)) {
                        _animator.SetTrigger("Rotation_Y_Right");
                    }
                    else if (rotatbleComp._isRotateStartFream && (rotatbleComp.offsetRotAxis.y == -1)) {
                        _animator.SetTrigger("Rotation_Y_Left");
                    }
                }

                // ボルトブロック操作
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {

                    _stricRotAngle.StickRotY_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY, this.transform);
                }

                // 高速回転はスティックの右左倒した自転で始まる
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.SpinObject) {

                    // R3押し込み時の処理
                    if (_rotationSpinButton.WasPressedThisFrame()) {

                        if (rotatbleComp._isSpining) {
                            Debug.Log("高速回転終了");
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

                Debug.Log("左右オブジェクト取得中");

                // ブロックごとに回転の種類が違うのでKindを使って仕分ける
                // 通常の黄色ブロックと磁石ブロックはスティックでの操作にて行う
                if (rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.NomalRotObject || rotatbleKind._RotObjKind == RotObjkinds.ObjectKind.UnionRotObject) {

                    // スティック回転X
                    _stricRotAngle.StickRotX_Update();
                    rotatbleComp.StickRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, _stricRotAngle.GetStickDialAngleX, this.transform);
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
    private GameObject BlockPriority(in RotObjHitCheck _bottom, in RotObjHitCheck _front) {

        // 下にオブジェクトがあるか確認
        if (_bottom.GetRotObj != null) {

            _blockPriorty = BlockPriorty.Bottom;
            return _bottom.GetRotObj;
        }
        else if (_front.GetRotObj != null) {
            _blockPriorty = BlockPriorty.Front;
            return _front.GetRotObj;
        }
        else {
            _blockPriorty = BlockPriorty.None;
            return null;
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

                    // 左右にボルトがある場合はYのアニメーションの再生をする
                    if (_frontHitCheck.GetRotObj.GetComponent<RotObjkinds>()._RotObjKind == RotObjkinds.ObjectKind.BoltRotObject) {
                        _yBlockLock = true;
                    }
                    else {
                        _xBlockLock = true;
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
        if(_bottom.GetRotObj != null) {
            _LockFreamObj.transform.position = _bottom.GetRotPartsObj.transform.position;
            _LockFreamObj.active = true;
        }
        else if(_front.GetRotObj != null) {
            _LockFreamObj.transform.position = _front.GetRotPartsObj.transform.position;
            _LockFreamObj.active = true;
        }
        else {
            _LockFreamObj.active = false;
        }
    }

    private void LockFreamMassSetActive(GameObject _object, bool _displayflg) {

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



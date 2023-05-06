using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class StickRotAngle : MonoBehaviour {

    void Start() {

        // InputSystemを取得
        _playerInput = GetComponent<PlayerInput>();
    }

    // 回転オブジェクトのY軸処理
    public void StickRotAngleY_Update() {
        _stickDialAngle_Y = SettingDialAngle(GetAngleY());
    }

    // 回転オブジェクトのX軸処理
    public void StickRotAngleX_Update() {
        _stickDialAngle_X = SettingDialAngle(GetAngleX());
    }
}

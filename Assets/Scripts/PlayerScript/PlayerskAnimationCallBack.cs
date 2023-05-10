using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerskAnimationCallBack : MonoBehaviour{

    // 回転アニメーション中フラグ
    private bool _isRotationAnimPlay = false;

    // 実際に回転できるようになるまでのフラグ
    private bool _isRotationValid = false;

    public void AnimRotStart() {
        Debug.Log("回転アニメーション開始");
        _isRotationAnimPlay = true;
    }

    public void AnimRotEnd() {
        Debug.Log("回転アニメーション終了");
        _isRotationAnimPlay = false;
    }

    public void RotationValid() {
        Debug.Log("回転動作可能状態");
        _isRotationValid = true;
    }

    public void RotationInValid() {
        _isRotationValid = false;
    }

    // Getter
    public bool GetIsRotationAnimPlay {
        get { return _isRotationAnimPlay; }
    }

    public bool GetIsRotationValid{
        get{ return _isRotationValid; }
    }
}

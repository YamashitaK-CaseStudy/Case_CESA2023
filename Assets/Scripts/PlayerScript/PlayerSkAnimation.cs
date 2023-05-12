using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    [SerializeField] private GameObject _skeletalObj;
    [SerializeField] private PlayerskAnimationCallBack _animCallBack;

    private Animator _animator;

    // アニメーション側で使う変数
    private bool _yBlockLock = false, _yBlockUpperLock = false, _xBlockLock = false;
   
    private void PlayerSkAnimationStart() {

        _animator = _skeletalObj.GetComponent<Animator>();  
    }

    private void PlayerSkAnimationUpdate() {

        // y軸回転開始アニメーション
        if (_yBlockLock) {
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
            _animator.SetBool("StartRot_Y", true);
        }
        // 頭上にブロックがある時の回転開始アニメーション
        else if (_yBlockUpperLock) {
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
            _animator.SetBool("StartRotUpper_Y", true);
        }
        // x軸回転開始アニメーション
        else if (_xBlockLock) {
            _animator.SetBool("StartRot_X", true);
        }
        else {
            _animator.SetBool("StartRot_Y", false);
            _animator.SetBool("StartRot_X", false);
            _animator.SetBool("StartRotUpper_Y", false);
        }

        // y軸回転時のアニメーション
        if (_bottomHitCheck.GetRotObj != null) {

            if (_bottomHitCheck.GetRotObj.GetComponent<RotatableObject>()._isRotateStartFream) {
                _animator.SetTrigger("Rotation_Y");
            }
        }


        // 速度が0以上
        var absMove = Mathf.Abs(_speedx);
        if (absMove > 0) {

            _animator.SetBool("RunState", true);
            _animator.SetFloat("RunSpeed", absMove * 0.3f);
        }
        else {

            _animator.SetBool("RunState", false);
            _animator.SetFloat("RunSpeed", 0);
        }

       
        // ジャンプ中に頭にブロックが当たったらボール状態にする
        if(!_groundCheck.IsGround && _upperrayCheck.IsUpperHit) {
            _animator.SetTrigger("UpperHit");
        }
    }
}

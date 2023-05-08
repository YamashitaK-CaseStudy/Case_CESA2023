using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    [SerializeField] private GameObject _skeletalObj;
    [SerializeField] private float _idleCheneTime;   // 通常アイドル状態に戻る時間

    private Animator _animator;
    private TimeMeasurement.Alarm _idleChangeTimer;
    private TimeMeasurement.Alarm _xAxisRotWaitTimer;

    private void PlayerSkAnimationStart() {

        _animator = _skeletalObj.GetComponent<Animator>();
       
         _idleChangeTimer = _timeMeasurement.AddArarm("IdleChange", _idleCheneTime);
        _idleChangeTimer.TimeStart = true;

        _xAxisRotWaitTimer = _timeMeasurement.AddArarm("xRotWait", 1.5f);
    }

    private void PlayerSkAnimationUpdate() {

        // 速度が0以上
        var absMove = Mathf.Abs(Speed_x);
        if (absMove > 0) {

            _animator.SetBool("RunState", true);
            _animator.SetFloat("RunSpeed", absMove * 0.3f);
            _idleChangeTimer.ResetTime();
        }
        else {

            _animator.SetFloat("RunSpeed", 0);

            // 頭上にブロックがないかつアイドル状態に変更する時間が来てたら通常アイドル状態に戻る
            if (_idleChangeTimer.TimeEnd) {
                if (!_upperrayCheck.IsUpperHit) {
                    _animator.SetBool("RunState", false);
                    _animator.SetBool("UpBlock", false);
                }
                else if (_upperrayCheck.IsUpperHit) {
                    _animator.SetBool("RunState", false);
                    _animator.SetBool("UpBlock", true);
                }
            }
        }

        // 通常アイドル状態からジャンプしたときのアニメーションを再生
        if (_isnormalIdle_To_Jump || _isBall_To_Jump) {
            _isnormalIdle_To_Jump = false;
            _isBall_To_Jump = false;

            Debug.Log("ジャンプ処理");
            _animator.SetTrigger("StartJump");
        }

        // y軸回転時のアニメーション
        if (_priortyAxis == priorityAxis.yAxisRot) {
            if (_bottomHitCheck.GetIsRotHit) {
                if (_bottomHitCheck.GetRotObj.GetComponent<RotatableObject>()._isRotateStartFream) {
                    _animator.SetBool("RunState", false);
                    _animator.SetTrigger("StartRot_Y");
                }
            }
        }

        // x軸回転時のアニメーション
        if (_priortyAxis == priorityAxis.xAxisRot) {
            if (_stricRotAngle.GetIsActicStick) {

                _animator.SetBool("StartRot_X", true);
                _xAxisRotWaitTimer.TimeStart = true;

                if (_xAxisRotWaitTimer.TimeEnd) {
                    _stricRotAngle._isActicDial_X = true;
                }
            }
            else {
                _animator.SetBool("StartRot_X", false);
                _xAxisRotWaitTimer.ResetTime();
                _xAxisRotWaitTimer.TimeStart = false;
                _stricRotAngle._isActicDial_X = false;
            }
        }
    }
}

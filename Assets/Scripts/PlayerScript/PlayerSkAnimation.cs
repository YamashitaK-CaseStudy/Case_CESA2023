using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class Player : MonoBehaviour{

    [SerializeField] private GameObject _skeletalObj;
    [SerializeField] private PlayerskAnimationCallBack _animCallBack;
    [SerializeField] private VisualEffect _jumpEffectRight,_jumpEffectLeft;
    [SerializeField] private VisualEffect _moveEffect;

    private Animator _animator;
    private bool _jumpEffectdoOnce = false;
    private List<GameObject> _MoveEffectList = new List<GameObject>();

    public Animator GetAnimator {
        get { return _animator; }
    }

    // アニメーション側で使う変数
    private bool _yBlockLock = false, _yBlockUpperLock = false, _xBlockLock = false;
   
    private void PlayerSkAnimationStart() {
       
        _animator = _skeletalObj.GetComponent<Animator>();
    }

    GameObject _lastCloneMoveEffect = null;

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

        // ジャンプ中に頭にブロックが当たったらボール状態にする
        if (!_groundCheck.IsGround && _upperrayCheck.IsUpperHit) {
            _animator.SetTrigger("UpperHit");
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

       
        //Debug.Log("速度" + Math.Abs(_speedx));
        // 移動時のエフェクト起動
        if (Math.Abs(_speedx) > 0 && _groundCheck.IsGround) {
            // プレイヤーが反転した時煙生成
            var moveEffect = Instantiate(_moveEffect.gameObject, _moveEffect.transform.position, transform.rotation);
            moveEffect.GetComponent<VisualEffect>().SendEvent("PlayEffect");
            moveEffect.GetComponent<EffectEndDestroy>().EffctStopTimerStart();
        }

        //Debug.Log("エフェクトの個数" + _MoveEffectList.Count);

        // ジャンプ中エフェクト発生
        if (_animCallBack.GetIsJumpEffectPlay) {
         
            if (!_jumpEffectdoOnce) {
                Debug.Log("発生中");
                //_jumpEffect.SendEvent("StopEffect");
                //_jumpEffect.SendEvent("PlayEffect");

                var jumpEffectL = Instantiate(_jumpEffectLeft.gameObject,  _jumpEffectLeft.transform.position,  _jumpEffectLeft.transform.rotation);
                var jumpEffectR = Instantiate(_jumpEffectRight.gameObject, _jumpEffectRight.transform.position, _jumpEffectRight.transform.rotation);
                jumpEffectL.GetComponent<VisualEffect>().SendEvent("PlayEffect");
                jumpEffectR.GetComponent<VisualEffect>().SendEvent("PlayEffect");
                jumpEffectL.GetComponent<EffectEndDestroy>().EffctStopTimerStart();
                jumpEffectR.GetComponent<EffectEndDestroy>().EffctStopTimerStart();

                _jumpEffectdoOnce = true;
            }
        }
        else {
            //_jumpEffect.SendEvent("StopEffect");
            _jumpEffectdoOnce = false;
        }
    }
}

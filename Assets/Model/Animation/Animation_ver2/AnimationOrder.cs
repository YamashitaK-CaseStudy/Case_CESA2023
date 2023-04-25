using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOrder : MonoBehaviour{

    private Player _player;
    private Animator _animator;

    void Start(){

        // PlayerScriptを取得する
        _player = transform.root.gameObject.GetComponent<Player>();

        // Animatorを取得
        _animator = GetComponent<Animator>();
    }

    void Update(){

        // 移動アニメーション
        if (Mathf.Abs(_player.GetMoveVelocity.x) > 0) {

            _animator.SetBool("Run", true);
        }
        else {

            _animator.SetBool("Run", false);

            if (_player.IsUpBlock()) {
                _animator.SetBool("UpBlock", true);
            }
            else {
                _animator.SetBool("UpBlock", false);
            }
        }

        // 回転アニメーションY
        if (_player._startrotFreamY) {
            _animator.SetTrigger("StartRot_Y");
        }

        // 回転アニメーションX
        if (_player._startrotFreamX) {
            _animator.SetBool("StartRot_X",true);
        }
        else {
            _animator.SetBool("StartRot_X", false);
        }

        // ジャンプアニメーション
        if (_player._startJump) {
            _animator.SetTrigger("StartJumo");
        }
    }
}

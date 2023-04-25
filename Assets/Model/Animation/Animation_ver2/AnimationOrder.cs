using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOrder : MonoBehaviour{

    private Player _player;
    private Animator _animator;

    void Start(){

        // PlayerScript���擾����
        _player = transform.root.gameObject.GetComponent<Player>();

        // Animator���擾
        _animator = GetComponent<Animator>();
    }

    void Update(){

        // �ړ��A�j���[�V����
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

        // ��]�A�j���[�V����Y
        if (_player._startrotFreamY) {
            _animator.SetTrigger("StartRot_Y");
        }

        // ��]�A�j���[�V����X
        if (_player._startrotFreamX) {
            _animator.SetBool("StartRot_X",true);
        }
        else {
            _animator.SetBool("StartRot_X", false);
        }

        // �W�����v�A�j���[�V����
        if (_player._startJump) {
            _animator.SetTrigger("StartJumo");
        }
    }
}

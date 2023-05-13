using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerskAnimationCallBack : MonoBehaviour{

    // ��]�A�j���[�V�������t���O
    private bool _isRotationAnimPlay = false;

    // ���ۂɉ�]�ł���悤�ɂȂ�܂ł̃t���O
    private bool _isRotationValid = false;

    // ��W�����v�o����܂ł̑҂��t���O
    private bool _isBigJumpValid = false;

    public void AnimRotStart() {
        //Debug.Log("��]�A�j���[�V�����J�n");
        _isRotationAnimPlay = true;
    }

    public void AnimRotEnd() {
        //Debug.Log("��]�A�j���[�V�����I��");
        _isRotationAnimPlay = false;
    }

    public void RotationValid() {
        //Debug.Log("��]����\���");
        _isRotationValid = true;
    }

    public void RotationInValid() {
        _isRotationValid = false;
    }

    // ��W�����v�\���
    public void BigJumpVaild() {
        _isBigJumpValid = true;
    }

    // ��W�����v�\���
    public void BigJumpInVaild() {
        _isBigJumpValid = false;
    }

    // Getter
    public bool GetIsRotationAnimPlay {
        get { return _isRotationAnimPlay; }
    }

    public bool GetIsRotationValid{
        get{ return _isRotationValid; }
    }

    public bool GetIsBigJumpVaild {
        get { return _isBigJumpValid; }
    }
}

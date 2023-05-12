using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerskAnimationCallBack : MonoBehaviour{

    // ��]�A�j���[�V�������t���O
    private bool _isRotationAnimPlay = false;

    // ���ۂɉ�]�ł���悤�ɂȂ�܂ł̃t���O
    private bool _isRotationValid = false;

    public void AnimRotStart() {
        Debug.Log("��]�A�j���[�V�����J�n");
        _isRotationAnimPlay = true;
    }

    public void AnimRotEnd() {
        Debug.Log("��]�A�j���[�V�����I��");
        _isRotationAnimPlay = false;
    }

    public void RotationValid() {
        Debug.Log("��]����\���");
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

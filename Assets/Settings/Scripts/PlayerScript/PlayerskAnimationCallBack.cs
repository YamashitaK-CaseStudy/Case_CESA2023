using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerskAnimationCallBack : MonoBehaviour{

    // ��]�A�j���[�V�������t���O
    private bool _isRotationAnimPlay = false;

    // ���ۂɉ�]�ł���悤�ɂȂ�܂ł̃t���O
    private bool _isRotationValid = false;

    // ��W�����v�o����܂ł̑҂��t���O
    private bool _isBigJumpValid = false;

    // �W�����v���̃G�t�F�N�g�t���O
    private bool _isJumpEffectPlay = false;

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

    // ���b�NSE
    public void PlayerSE_Lock() {
        Debug.Log("���b�NSE");
        PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.Lock);
    }

    // �v���C���[��]DE
    public void PlayerSE_Rotation() {
        Debug.Log("�v���C���[��]SE");
        PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.Rotation);
    }

    // �v���C���[�W�����v���̃G�t�F�N�g�N���J�n
    public void PlayerJumpEffectPlay() {
        // �G�t�F�N�g���N������
        _isJumpEffectPlay = true;
    }

    // �v���C���[�W�����v���̃G�t�F�N�g��~
    public void PlayerJumpEffectStop() {
        // �G�t�F�N�g���N������
         _isJumpEffectPlay = false;
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

    public bool GetIsJumpEffectPlay {
        get { return _isJumpEffectPlay; }
    }
}

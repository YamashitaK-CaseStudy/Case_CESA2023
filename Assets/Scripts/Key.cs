using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Key : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    [SerializeField] private Vector3 _animOffsetMove;
    private bool isAnimation = false;

    void Update() {

        // ��ɉ�]
        this.transform.rotation *= Quaternion.AngleAxis(_rotSpeed,Vector3.up);
    }

    // �����v���C���[�Ɏ擾���ꂽ���Ă΂��֐�
    public void ToBeObtained() {

        // �擾���ꂽ�献��A���Ŏ擾�ł��Ȃ��悤�ɓ����蔻�������
        this.GetComponent<BoxCollider>().enabled = false;

        this.transform.DOMove(_animOffsetMove, 0.8f).SetRelative().OnComplete(AnimationComplete);
    }
    
    // �A�j���[�V���������������玩�g���폜����
    private void AnimationComplete() {

        Destroy(this.gameObject);
    }
}

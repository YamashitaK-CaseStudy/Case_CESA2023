using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BlinklingText : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI _tmp;

    [Header("1���[�v�̒���(�b�P��)")]
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float _duration = 1.0f;

    //�J�n���̐F�B
    [Header("���[�v�J�n���̐F")]
    [SerializeField]
    Color32 _startColor = new Color32(255, 255, 255, 255);

    //�I��(�܂�Ԃ�)���̐F�B
    [Header("���[�v�I�����̐F")]
    [SerializeField]
    Color32 _endColor = new Color32(255, 255, 255, 64);



    //�C���X�y�N�^�[����ݒ肵���ꍇ�́AGetComponent����K�v���Ȃ��Ȃ�ׁAAwake���폜���Ă��ǂ��B
    void Awake()
    {
        if (_tmp == null)
            _tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _tmp.color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(Time.time / _duration, 1.0f));
    }

    void Enter()
	{
        
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour
{

    [SerializeField] private Vector3 _startCenterPosBG;
    [SerializeField] private Vector3 _endCenterPosBG;
    [SerializeField] public GameObject _player;
    [SerializeField] public GameObject _goal;

    private float _movingRangeBG;
    private float _distanceToGoal;
    private float _startPlayerPosX;
    private float _goalPosX;

    // Start is called before the first frame update
    void Start()
    {
        // �w�i�̈ړ�����v�Z
        _movingRangeBG = Mathf.Abs(_startCenterPosBG.x - _endCenterPosBG.x);
        _startPlayerPosX = _player.transform.position.x;
        
        // �v���C���[�̃S�[���܂ł�X���ɂ����Ă̋������v�Z
        _goalPosX= _goal.transform.position.x;
        _distanceToGoal = Mathf.Abs(_startPlayerPosX - _goalPosX);
    }

    // Update is called once per frame
    void Update(){
        /*
         */
        // ���݂̃v���C���[��X���W���擾
        float nowPlayerPosX = _player.transform.position.x;

        // �v���C���[�̐i�񂾋��������߂�
        float distanceTraveled = Mathf.Abs(nowPlayerPosX - _startPlayerPosX);

        // ���݂̃v���C���[�̐i�s�x���v�Z�F�����ŏo��
        float progressionToGoal = distanceTraveled/_distanceToGoal;
        //Debug.Log(progressionToGoal);

        // �w�i�̈ړ��ʂ��v�Z
        float amountMovementBG = _movingRangeBG * progressionToGoal;

        // ���
        Vector3 nowPosBG = _startCenterPosBG;
        nowPosBG.x -= amountMovementBG;
        this.transform.localPosition = nowPosBG;
        

        //this.transform.position = nowPosBG;


    }
}

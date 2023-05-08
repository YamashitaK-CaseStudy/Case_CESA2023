using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    // �v���C���[�ɂ��Ă�u���b�N���m�����蔻��I�u�W�F�N�g��t����
    [SerializeField] private GameObject _frontColliderObj;
    [SerializeField] private GameObject _bottomColliderObj;
    [SerializeField] private GameObject _groundColliderObj;
    [SerializeField] private GameObject _frontrayColliderObj;
    [SerializeField] private GameObject _upperrayColliderObj;

    private RotObjHitCheck _frontHitCheck = null;
    private RotObjHitCheck _bottomHitCheck = null;
    private GroundCheck _groundCheck = null;
    private FrontRayCheck _frontrayCheck = null;
    private UpperRayCheck _upperrayCheck = null;

    private void PlayerBlockColliderStart() {

        // �����蔻��̎擾
        _frontHitCheck  = _frontColliderObj.GetComponent<RotObjHitCheck>();
        _bottomHitCheck = _bottomColliderObj.GetComponent<RotObjHitCheck>();
        _groundCheck    = _groundColliderObj.GetComponent<GroundCheck>();
        _frontrayCheck  = _frontrayColliderObj.GetComponent<FrontRayCheck>();
        _upperrayCheck = _upperrayColliderObj.GetComponent<UpperRayCheck>();
    }

    private void PlayerBlockColliderUpdate() {

        // Front��]�I�u�W�F�N�g���؂�ւ������
        if (_frontHitCheck.GetIsChangeRotHit) {
            _frontHitCheck.InitChangeRotHit();

            // �؂�ւ��^�C�~���O
            Debug.Log("�t�����g�؂�ւ��^�C�~���O");
            _stricRotAngle._isDamiObjCreate = false;
            Destroy(_stricRotAngle._damiObject);

            _stricRotAngle.xAxisManyObjJude(_frontHitCheck);
        }

        // Bottom��]�I�u�W�F�N�g���؂�ւ������
        if (_bottomHitCheck.GetIsChangeRotHit) {
            _bottomHitCheck.InitChangeRotHit();

            // �؂�ւ��^�C�~���O
            Debug.Log("�{�g���؂�ւ��^�C�~���O");
            _stricRotAngle._isDamiObjCreate = false;
             Destroy(_stricRotAngle._damiObject);

            _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
        }
    }
}

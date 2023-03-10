using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��]�ł���I�u�W�F�N�g�̃X�N���v�g
public class RotatableObject : MonoBehaviour{

    [SerializeField] Vector3 _rotAxis;            // ��]���̌����x�N�g��
    [SerializeField] Vector3 _axisCenterLocalPos; // �����S���W:���[�J�����W�Ŏw�肵�Ă�������
    [SerializeField] float _axisLength;

    private Vector3 _axisCenterWorldPos;

    private Quaternion _rotQuat;  // ��]�̃N�I�[�^�j�I��
    private Quaternion _baceQuat; // ��]�͂��߂̃N�I�[�^�j�I��

    private bool _isSpin = false; // ��]���Ă��邩�t���O

    // Start is called before the first frame update
    void Start(){

        _rotAxis.Normalize();

        // ���̒��S�̃��[���h���W���v�Z
        CalkAxisWorldPos();
     

    }


    void CalkAxisWorldPos() {
        // �I�u�W�F�N�g�ŗL�̎�������
        // LineRenderer�R���|�[�l���g���擾
        var lineRenderer = this.GetComponent<LineRenderer>();

        // �������[���h���W�Y�ɕϊ�
        _axisCenterWorldPos = transform.TransformPoint(_axisCenterLocalPos); // �����W���v�Z

        var axisHalfLength = _axisLength / 2;

        var rotAxisStartPos = _axisCenterWorldPos + ( axisHalfLength * _rotAxis);
        var rotAxisEndPos = _axisCenterWorldPos + ( - axisHalfLength * _rotAxis);

        var positions = new Vector3[]{
             rotAxisStartPos, // �J�n�_
             rotAxisEndPos    // �I���_
        };

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);
    }

    // ���g�̎��ł܂킷��������
    public void RotateSmallAxisSelf() {
        var tr = this.transform;
        
        // ��]�̃N�H�[�^�j�I���쐬
        var rotQuat = Quaternion.AngleAxis(180, _rotAxis);
     

        // �~�^���̈ʒu�v�Z
        var pos = tr.position;
        pos -= _axisCenterWorldPos;
        pos = rotQuat * pos;
        pos += _axisCenterWorldPos;

        tr.position = pos;

        // �����X�V
        tr.rotation = tr.rotation * rotQuat;
         
        
    }

    // �O���̎��ł܂킷��������
    public void RotateSmallAxisExtern(Vector3 centerPos) {
        // ��]�̒��S���W�Ǝ��g�̍��W�ԂŃx�N�g�����Ƃ�
        var tmpVec = centerPos - this.transform.position; 

        // Z���������ƊO�ς�����ĉ�]�������߂�
        var rotAxis = Vector3.Cross(centerPos, tmpVec);

        var tr = this.transform;

        // ��]�̃N�H�[�^�j�I���쐬
        var rotQuat = Quaternion.AngleAxis(180, rotAxis);

        var dirQuat = Quaternion.AngleAxis(180,Vector3.up);


        // �~�^���̈ʒu�v�Z
        var pos = tr.position;
        pos -= centerPos;
        pos = rotQuat * pos;
        pos += centerPos;

        tr.position = pos;

        // �����X�V
        tr.rotation = tr.rotation * dirQuat;
  
        CalkAxisWorldPos();
    }

    public void SpinAxisSelf() {
        _isSpin = true;

        // ��]�̃N�H�[�^�j�I���쐬
        _rotQuat = Quaternion.AngleAxis(180, _rotAxis);
    }

    public void SpinAxisExturn(Vector3 spinCenterPos) {
    
    
        
    }

    // Update is called once per frame
    void Update(){
        if ( _isSpin ) {
            var tr = this.transform;

            // �~�^���̈ʒu�v�Z
            var pos = tr.position;
            pos -= _axisCenterWorldPos;
            pos = _rotQuat * pos;
            pos += _axisCenterWorldPos;

            tr.position = pos;

            // �����X�V
            tr.rotation = tr.rotation * _rotQuat;
        }
    }


}

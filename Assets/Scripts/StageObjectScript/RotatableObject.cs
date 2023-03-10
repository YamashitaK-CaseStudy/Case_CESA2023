using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��]�ł���I�u�W�F�N�g�̃X�N���v�g
public class RotatableObject : MonoBehaviour{

    [SerializeField] Vector3 _rotAxis;            // ��]���̌����x�N�g��
    [SerializeField] Vector3 _axisCenterLocalPos; // �����S���W:���[�J�����W���Ŏw�肵�Ă�������
    [SerializeField] float _axisLength;

    private Vector3 _axisCenterWorldPos;

    // Start is called before the first frame update
    void Start(){

        _rotAxis.Normalize();
        
        // �I�u�W�F�N�g�ŗL�̎�������
        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        var lineRenderer = this.GetComponent<LineRenderer>();

        var selfPos = this.transform.position;  // ���g�̍��W���擾(���S)
        _axisCenterWorldPos = selfPos + _axisCenterLocalPos; // �����W���v�Z

        var axisHalfLength = _axisLength / 2;

        var rotAxisStartPos = _axisCenterWorldPos + ( axisHalfLength * _rotAxis);
        var rotAxisEndPos = _axisCenterWorldPos + ( - axisHalfLength * _rotAxis);

        var positions = new Vector3[]{
             rotAxisStartPos, // �J�n�_
             rotAxisEndPos    // �I���_
        };

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions); 
        
        //RotateSmallAxisSelf();
    }

    // ���g�̎��ł܂킷��������
    void RotateSmallAxisSelf() {
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
    void RotateSmallAxisExtern(Vector3 rotAxisExturn,Vector3 centerPos) {
        var tr = this.transform;

        // ��]�̃N�H�[�^�j�I���쐬
        var rotQuat = Quaternion.AngleAxis(180, rotAxisExturn);

        // �~�^���̈ʒu�v�Z
        var pos = tr.position;
        pos -= centerPos;
        pos = rotQuat * pos;
        pos += centerPos;

        tr.position = pos;

        // �����X�V
        tr.rotation = tr.rotation * rotQuat;

    }

    // Update is called once per frame
    void Update(){
       
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��]�ł���I�u�W�F�N�g�̃X�N���v�g
public partial class RotatableObject : MonoBehaviour{

    [SerializeField] private Vector3 _selfRotAxis;              // ���g�̉�]���x�N�g��
    [SerializeField] private Vector3 _axisCenterLocalPos;       // �����S���W:���[�J�����W�Ŏw�肵�Ă�������
    [SerializeField] private float _axisLength;                 // TEST�F���̒���
    [SerializeField] private float _rotRequirdTime = 1.0f;      // 1��]�ɕK�v�Ȏ���(sec)
    private Vector3 _axisCenterWorldPos; // ��]���̒��S�̃��[���h���W


    private float _elapsedTime = 0.0f;  // �o�ߎ���
    private bool _isRotating = false;   // ��]���Ă邩�t���O
    private bool _isSpin = false;       // ��]���Ă��邩�t���O

    // Start is called before the first frame update
    void Start(){

        // ���g�̉�]���̌����𐳋K�����Ƃ�
        _selfRotAxis.Normalize();

        // ���̒��S�̃��[���h���W���v�Z
        CalkAxisWorldPos();

        // �܂킷��̐ݒ�
        StartSettingSpin();

    }

    // ���g�̎��̃��[���h���W���v�Z����
    void CalkAxisWorldPos() {
        // �I�u�W�F�N�g�ŗL�̎�������
        // LineRenderer�R���|�[�l���g���擾
        var lineRenderer = this.GetComponent<LineRenderer>();

        // �������[���h���W�Y�ɕϊ�
        _axisCenterWorldPos = transform.TransformPoint(_axisCenterLocalPos); // �����W���v�Z

        var axisHalfLength = _axisLength / 2;

        var rotAxisStartPos = _axisCenterWorldPos + ( axisHalfLength * _selfRotAxis);
        var rotAxisEndPos = _axisCenterWorldPos + ( - axisHalfLength * _selfRotAxis);

        var positions = new Vector3[]{
             rotAxisStartPos, // �J�n�_
             rotAxisEndPos    // �I���_
        };

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);
    }

   

    // Update is called once per frame
    void Update(){
        UpdateRotate();
        UpdateSpin();      
    }


}

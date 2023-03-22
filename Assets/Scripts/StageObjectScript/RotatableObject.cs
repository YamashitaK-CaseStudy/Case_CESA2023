using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��]�ł���I�u�W�F�N�g�̃X�N���v�g
public class RotatableObject : MonoBehaviour{

    [SerializeField] private Vector3 _selfRotAxis;              // ���g�̉�]���x�N�g��
    [SerializeField] public Vector3 _axisCenterLocalPos;        // �����S���W:���[�J�����W�Ŏw�肵�Ă�������
    [SerializeField] private float _axisLength;                 // TEST�F���̒���
    [SerializeField] private float _rotRequirdTime = 1.0f;      // 1��]�ɕK�v�Ȏ���(sec)
    
    private float _elapsedTime = 0.0f;  // �o�ߎ���

    private Vector3 _axisCenterWorldPos; // ��]���̒��S�̃��[���h���W


    public bool _isRotating = false;   // ��]���Ă邩�t���O
    private bool _isSpin = false;       // ��]���Ă��邩�t���O

    // Start is called before the first frame update
    void Start(){

        // ���g�̉�]���̌����𐳋K�����Ƃ�
        _selfRotAxis.Normalize();

        // ���̒��S�̃��[���h���W���v�Z
        CalkAxisWorldPos();
     

    }

    // ���̍��W���v�Z����
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

    // ���g�̎��ł܂킷��������
    public void RotateSmallAxisSelf() {
        if ( _isSpin || _isRotating ) {
            return;
        }
       
        // �t���O�𗧂Ă�
        _isRotating = true;

        _elapsedTime = 0.0f;
    }

    // �O���̎��ł܂킷��������
    public void RotateSmallAxisExtern(Vector3 centerPos,Vector3 rotAxis) {
        // ��]�̒��S���W�Ǝ��g�̍��W�ԂŃx�N�g�����Ƃ�
        var tmpVec = centerPos - this.transform.position; 

        // Z���������ƊO�ς�����ĉ�]�������߂�
      

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

    // ���g�̍��W�ł܂킷�������
    public void SpinAxisSelf() {
        _isSpin = true;
    }

    // �O���̎��ł܂킷�������
    public void SpinAxisExturn(Vector3 spinCenterPos,Vector3 spinAxisVec) {
         
    }

    // Update is called once per frame
    void Update(){
        // ��]�����t���O
        if ( _isRotating ) {

            // ���N�G�X�g�f���^�^�C�������߂�
            // ���N�G�X�g�f���^�^�C���F�f���^�^�C����1��]�ɕK�v�Ȏ��ԂŊ������l
            // ����̍��Z�l��1�ɂȂ�����,1��]�ɕK�v�Ȏ��Ԃ��o�߂������ƂɂȂ�
            float requiredDeltaTime = Time.deltaTime/_rotRequirdTime;
            _elapsedTime += requiredDeltaTime;

            // �ڕW��]��*���N�G�X�g�f���^�^�C���ł��̃t���[���ł̉�]�p�x�����߂邱�Ƃ��ł���
            // ���N�G�X�g�f���^�^�C���̍��Z�l�����傤��1�ɂȂ�悤�ɕ␳��������Ƒ���]�ʂ͖ڕW��]�ʂƈ�v����
            if ( _elapsedTime >= 1 ) {
                _isRotating = false;
                requiredDeltaTime -= ( _elapsedTime - 1 ); // �␳
			}

            // ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
            var angleAxis = Quaternion.AngleAxis(180 * requiredDeltaTime, _selfRotAxis);

            // �~�^���̈ʒu�v�Z
            var tr = transform;
            var pos = tr.position;

            pos -= _axisCenterWorldPos;
            pos = angleAxis * pos;
            pos += _axisCenterWorldPos;

            tr.position = pos;

            tr.rotation = tr.rotation * angleAxis;
        }

        // ����Ă��邩�t���O
        if ( _isSpin ) {
        }
    }


}

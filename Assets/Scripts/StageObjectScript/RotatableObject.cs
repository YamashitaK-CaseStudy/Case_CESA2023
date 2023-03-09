using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��]�ł���I�u�W�F�N�g�̃X�N���v�g
public class RotatableObject : MonoBehaviour{

    [SerializeField] Vector3 _rotAxis;            // ��]���̌����x�N�g��
    [SerializeField] Vector3 _axisCenterLocalPos; // �����S���W:���[�J�����W���Ŏw�肵�Ă�������
    [SerializeField] float _axisLength;

    

    // Start is called before the first frame update
    void Start(){
        
        // �I�u�W�F�N�g�ŗL�̎�������
        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        var lineRenderer = this.GetComponent<LineRenderer>();


        var selfPos = this.transform.position;  // ���g�̍��W���擾(���S)
        var axisCenterWorldPos = selfPos + _axisCenterLocalPos; // �����W���v�Z

        var rotAxisStartPos = axisCenterWorldPos + ((_axisLength/2)*_rotAxis);
        var rotAxisEndPos= axisCenterWorldPos + (-(_axisLength/2)*_rotAxis);

        var positions = new Vector3[]{
             rotAxisStartPos, // �J�n�_
             rotAxisEndPos    // �I���_
        };

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

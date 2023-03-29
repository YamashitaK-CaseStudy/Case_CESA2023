using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//�֐��݂̂��ƃG���[�ɂȂ�̂ŃN���X�ɓ����
public class Calculation
{
    /**
     * ��̃x�N�g���̊Ԃɂ�����X�����擾���܂�
     * @return 1����-1�͈̔͂Œl���Ԃ�܂��B
     * ���F�E��]�@���F����]
     * �P�A-�P�@�̏ꍇ�́@�X�O�x�ł�
     * �O�@�̏ꍇ�́@�O�x�܂��͂P�W�O�x�ł�
     */
    public static float CalculateAngleBetweenVector(Vector2 currentVec, Vector2 oldVec)
    {
        currentVec.Normalize();
        oldVec.Normalize();

        //�E��90�x��]������
        Vector2 work = oldVec;
        oldVec.x = work.y;
        oldVec.y = -work.x;

        return Vector2.Dot(oldVec, currentVec);
    }
}

//�֐����g���Ă��Ȃ��G���[���������߂̗�N���X
public class Exsample : MonoBehaviour
{

    public void Start()
    {
        angleSelect = GetComponent<PlayerInput>().currentActionMap["AngleSelect"];
    }

    public void Update()
    {
        /*��]���̍X�V*/
        Vector2 currentVec = angleSelect.ReadValue<Vector2>();

        float result = Calculation.CalculateAngleBetweenVector(currentVec, oldStickVector);

        if (result > 0)
        {
            //�E��]���Ă���
        }
        else if (result < 0)
        {
            //����]���Ă���
        }

        oldStickVector = currentVec;
    }

    private InputAction angleSelect;
    private Vector2 oldStickVector;
}




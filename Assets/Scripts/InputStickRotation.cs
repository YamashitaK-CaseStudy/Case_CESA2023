using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*���������v���C���[�N���X�Ɏ������遁��������BEGIN*/
//public void OnAngleSelect(InputAction.CallbackContext context)
//{
//    /*��]���̍X�V*/
//}

//private static Vector2 oldStickVectorNormalize;

/*���������v���C���[�N���X�Ɏ������遁��������END*/

/**
 * �X�e�B�b�N�̉�]��Ԃ��擾���܂�
 * @return 1����-1�͈̔͂Œl���Ԃ�܂��B
 * ���F�E��]�@���F����]
 * �X�O�x�ɋ߂��قǐ�Βl���傫���Ȃ�܂�
 * �O�̏ꍇ�̓x�N�g�������������A�܂��͔��΂������Ă��܂��B
 */
float CalculateStickRotation(Vector2 currentVec, Vector2 oldVec)
{
    currentVec.Normalize();
    oldVec.Normalize();

    //�E��90�x��]������
    Vector2 work = oldVec;
    oldVec.x = work.y;
    oldVec.y = -work.x;

    return Vector2.Dot(oldVec, currentVec);
}




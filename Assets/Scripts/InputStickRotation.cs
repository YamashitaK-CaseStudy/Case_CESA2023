using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*＝＝＝＝プレイヤークラスに実装する＝＝＝＝＝BEGIN*/
//public void OnAngleSelect(InputAction.CallbackContext context)
//{
//    /*回転情報の更新*/
//}

//private static Vector2 oldStickVectorNormalize;

/*＝＝＝＝プレイヤークラスに実装する＝＝＝＝＝END*/

/**
 * スティックの回転状態を取得します
 * @return 1から-1の範囲で値が返ります。
 * 正：右回転　負：左回転
 * ９０度に近いほど絶対値が大きくなります
 * ０の場合はベクトルが同じ向き、または反対を向いています。
 */
float CalculateStickRotation(Vector2 currentVec, Vector2 oldVec)
{
    currentVec.Normalize();
    oldVec.Normalize();

    //右に90度回転させる
    Vector2 work = oldVec;
    oldVec.x = work.y;
    oldVec.y = -work.x;

    return Vector2.Dot(oldVec, currentVec);
}




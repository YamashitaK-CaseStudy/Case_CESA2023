using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//関数のみだとエラーになるのでクラスに内包する
public class Calculation
{
    /**
     * 二つのベクトルの間における傾きを取得します
     * @return 1から-1の範囲で値が返ります。
     * 正：右回転　負：左回転
     * １、-１　の場合は　９０度です
     * ０　の場合は　０度または１８０度です
     */
    public static float CalculateAngleBetweenVector(Vector2 currentVec, Vector2 oldVec)
    {
        currentVec.Normalize();
        oldVec.Normalize();

        //右に90度回転させる
        Vector2 work = oldVec;
        oldVec.x = work.y;
        oldVec.y = -work.x;

        return Vector2.Dot(oldVec, currentVec);
    }
}

//関数が使われていないエラーを消すための例クラス
public class Exsample : MonoBehaviour
{

    public void Start()
    {
        angleSelect = GetComponent<PlayerInput>().currentActionMap["AngleSelect"];
    }

    public void Update()
    {
        /*回転情報の更新*/
        Vector2 currentVec = angleSelect.ReadValue<Vector2>();

        float result = Calculation.CalculateAngleBetweenVector(currentVec, oldStickVector);

        if (result > 0)
        {
            //右回転している
        }
        else if (result < 0)
        {
            //左回転している
        }

        oldStickVector = currentVec;
    }

    private InputAction angleSelect;
    private Vector2 oldStickVector;
}




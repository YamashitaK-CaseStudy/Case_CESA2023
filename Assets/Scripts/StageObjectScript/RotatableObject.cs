using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転できるオブジェクトのスクリプト
public partial class RotatableObject : MonoBehaviour{

       
    [SerializeField] private float _rotRequirdTime = 1.0f;      // 1回転に必要な時間(sec)

    protected Vector3 _rotAxis;             // 自身の回転軸ベクトル
    private Vector3 _axisCenterWorldPos;    // 回転軸の中心のワールド座標
    private float _elapsedTime = 0.0f;      // 回転開始からの経過時間
    public bool _isRotating = false;        // 回転してるかフラグ
    public bool _isSpin = false;            // 回転しているかフラグ

    //プロパティ。外部からのメンバ変数へのアクセスを定義するもの。ゲッターやセッターのようなもの。
    public float ProgressRate//進捗率という意味です。_elapsedTimeが単純に経過した時間ではなく全体時間で割った０～１の進捗率として扱われているためこの名前にしました。
    {
        get
        {
            return _elapsedTime;
        }
    }

    // Start is called before the first frame update
    void Start(){
        StartSettingOtherHit();
        // 自身の回転軸の向きを正規化しとく
        _rotAxis.Normalize();
        // まわす大の設定
        StartSettingSpin();

    }



    // Update is called once per frame
    void Update(){
        UpdateRotate();
        UpdateSpin();      
    }


}

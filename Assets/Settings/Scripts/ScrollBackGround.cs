using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour
{

    [SerializeField] private Vector3 _startCenterPosBG;
    [SerializeField] private Vector3 _endCenterPosBG;
    [SerializeField] public GameObject _player;
    [SerializeField] public GameObject _goal;

    private float _movingRangeBG;
    private float _distanceToGoal;
    private float _startPlayerPosX;
    private float _goalPosX;

    // Start is called before the first frame update
    void Start()
    {
        // 背景の移動域を計算
        _movingRangeBG = Mathf.Abs(_startCenterPosBG.x - _endCenterPosBG.x);
        _startPlayerPosX = _player.transform.position.x;
        
        // プレイヤーのゴールまでのX軸においての距離を計算
        _goalPosX= _goal.transform.position.x;
        _distanceToGoal = Mathf.Abs(_startPlayerPosX - _goalPosX);
    }

    // Update is called once per frame
    void Update(){
        /*
         */
        // 現在のプレイヤーのX座標を取得
        float nowPlayerPosX = _player.transform.position.x;

        // プレイヤーの進んだ距離を求める
        float distanceTraveled = Mathf.Abs(nowPlayerPosX - _startPlayerPosX);

        // 現在のプレイヤーの進行度を計算：割合で出る
        float progressionToGoal = distanceTraveled/_distanceToGoal;
        //Debug.Log(progressionToGoal);

        // 背景の移動量を計算
        float amountMovementBG = _movingRangeBG * progressionToGoal;

        // 代入
        Vector3 nowPosBG = _startCenterPosBG;
        nowPosBG.x -= amountMovementBG;
        this.transform.localPosition = nowPosBG;
        

        //this.transform.position = nowPosBG;


    }
}

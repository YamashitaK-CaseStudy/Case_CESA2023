using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    // プレイヤーについてるブロック検知当たり判定オブジェクトを付ける
    [SerializeField] private GameObject _frontColliderObj;
    [SerializeField] private GameObject _bottomColliderObj;
    [SerializeField] private GameObject _groundColliderObj;
    [SerializeField] private GameObject _frontrayColliderObj;
    [SerializeField] private GameObject _upperrayColliderObj;

    private RotObjHitCheck _frontHitCheck = null;
    private RotObjHitCheck _bottomHitCheck = null;
    private GroundCheck _groundCheck = null;
    private FrontRayCheck _frontrayCheck = null;
    private UpperRayCheck _upperrayCheck = null;

    private void PlayerBlockColliderStart() {

        // 当たり判定の取得
        _frontHitCheck  = _frontColliderObj.GetComponent<RotObjHitCheck>();
        _bottomHitCheck = _bottomColliderObj.GetComponent<RotObjHitCheck>();
        _groundCheck    = _groundColliderObj.GetComponent<GroundCheck>();
        _frontrayCheck  = _frontrayColliderObj.GetComponent<FrontRayCheck>();
        _upperrayCheck = _upperrayColliderObj.GetComponent<UpperRayCheck>();
    }

    private void PlayerBlockColliderUpdate() {

        // Front回転オブジェクトが切り替わった時
        if (_frontHitCheck.GetIsChangeRotHit) {
            _frontHitCheck.InitChangeRotHit();

            // 切り替えタイミング
            Debug.Log("フロント切り替えタイミング");
            _stricRotAngle._isDamiObjCreate = false;
            Destroy(_stricRotAngle._damiObject);

            _stricRotAngle.xAxisManyObjJude(_frontHitCheck);
        }

        // Bottom回転オブジェクトが切り替わった時
        if (_bottomHitCheck.GetIsChangeRotHit) {
            _bottomHitCheck.InitChangeRotHit();

            // 切り替えタイミング
            Debug.Log("ボトム切り替えタイミング");
            _stricRotAngle._isDamiObjCreate = false;
             Destroy(_stricRotAngle._damiObject);

            _stricRotAngle.yAxisManyObjJude(_bottomHitCheck);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    // プレイヤーについてるブロック検知当たり判定オブジェクトを付ける
    [SerializeField] private GameObject _frontColliderObj;
    [SerializeField] private GameObject _backColliderObj;
    [SerializeField] private GameObject _bottomColliderObj;
    [SerializeField] private GameObject _groundColliderObj;
    [SerializeField] private GameObject _frontrayColliderObj;
    [SerializeField] private GameObject _upperrayColliderObj;
    [SerializeField] private GameObject _LockFreamObj;
     
    private RotObjHitCheck _frontHitCheck = null;
    private RotObjHitCheck _bottomHitCheck = null;
    private BackCheck _backHItCheck = null;
    private GroundCheck _groundCheck = null;
    private FrontRayCheck _frontrayCheck = null;
    private UpperRayCheck _upperrayCheck = null;

    private void PlayerBlockColliderStart() {

        // 当たり判定の取得
        _frontHitCheck  = _frontColliderObj.GetComponent<RotObjHitCheck>();
        _bottomHitCheck = _bottomColliderObj.GetComponent<RotObjHitCheck>();
        _backHItCheck = _backColliderObj.GetComponent<BackCheck>();
        _groundCheck    = _groundColliderObj.GetComponent<GroundCheck>();
        _frontrayCheck  = _frontrayColliderObj.GetComponent<FrontRayCheck>();
        _upperrayCheck  = _upperrayColliderObj.GetComponent<UpperRayCheck>();
    }

    private void PlayerBlockColliderUpdate() {
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperRayCheck : MonoBehaviour {

    [SerializeField, Header("頭のリグオブジェクトを選択する")]
    private GameObject _headrigObj;

    [SerializeField]
    private OneRay[] _oneRays;

    [SerializeField]
    private bool _IsdebugRay;


    private bool _isupperHit;

    public bool IsUpperHit {
        get { return _isupperHit; }
    }

    void Start() {
    }

    void Update() {

        // 頭のリグの位置
        var _headpos = _headrigObj.transform.position;

        // 前方にRayを飛ばして壁の検知をする
        RaycastHit hitInfo;

        foreach(var myray in _oneRays) {

            // Rayのスタート位置を設定
            Ray ray = new Ray(_headpos + myray.offsetPos, Vector3.up * myray.distance);
           
            if (Physics.Raycast(ray, out hitInfo, myray.distance,13)) {

                if (hitInfo.collider.transform.root.gameObject.layer == 13) {

                    _isupperHit = true;
                    return;
                }
            }
            else {
                _isupperHit = false;
            }

            if (_IsdebugRay) {
                Debug.DrawRay(_headpos + myray.offsetPos, Vector3.up * myray.distance, Color.red);
            }
        }
    }

    // 商品クラスをシリアライズ
    [Serializable]
    class OneRay {
        // rayのスタート地点
        public Vector3 offsetPos;

        // reyの長さ
        public float distance;
    }
}

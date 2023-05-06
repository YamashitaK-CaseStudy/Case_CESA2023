using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontRayCheck : MonoBehaviour{

    [SerializeField]
    OneRay[] _oneRays;

    [SerializeField]
    private bool _IsdebugRay;


    //[SerializeField] private float _rayDistance;

    private Player _player;
    private bool _isfrontHit = false;

    public bool IsFrontHit {
        get { return _isfrontHit; }
    }

    void Start() {

        _player = transform.root.GetComponent<Player>();
    }

    void Update() {

        // 前方にRayを飛ばして壁の検知をする
        RaycastHit hitInfo;

        // Rayのスタート位置を設定
        foreach (var myray in _oneRays) {

            Ray ray = new Ray(transform.position + myray.offsetPos, new Vector3(_player.Speed_x, 0, 0).normalized * myray.distance);

            if (Physics.Raycast(ray, out hitInfo, myray.distance,13)) {

                if (hitInfo.collider.transform.root.gameObject.layer == 13) {

                    _isfrontHit = true;

                   // Debug.Log(hitInfo.collider.name);
                    return;
                }
            }
            else {
                _isfrontHit = false;
            }

            if (_IsdebugRay) {
                Debug.DrawRay(transform.position + myray.offsetPos, new Vector3(_player.Speed_x, 0, 0).normalized * myray.distance, Color.red);
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

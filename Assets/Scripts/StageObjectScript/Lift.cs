using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SuzumuraTomoki;
using System;

public class Lift : RotatableObject {
    // リフトを伸ばす向き
    public enum LiftDirection {
        縦, 横,
    }

    [SerializeField] private LiftDirection _liftDirection;
    [SerializeField] private GameObject _liftStickobj;
    [SerializeField] private GameObject _liftRideobj;
    [SerializeField] private int _stickLength;
    public int _rideBlockIndex;

    // リフトを構成してるパーツのオブジェクトの可変長配列
    private List<GameObject> _liftStickList = new List<GameObject>();
    private GameObject _damiObj, _rideObj, playerObj;
    private float _liftSpeed = 0.5f;
    private int _nowRideBlock = 0;
    private bool _isMove = false;
 
    // エディタによる更新
    [System.Obsolete]
    public void ApplyInspector() {

        // 一旦全要素削除する
        DestroyStickObject(_liftStickList);

        // スティックの生成
        CreateStick(_liftDirection, _stickLength, _liftStickList);

        // リフトの乗る部分の生成
        CreateRide(_rideBlockIndex, _liftStickList);
    }

    private void CreateStick(in LiftDirection _dir, in int length, List<GameObject> _list) {

        _damiObj.SetActive(false);

        for (int i = 0; i < length; i++) {

            var stickobj = Instantiate(_liftStickobj, this.transform.GetChild(0));

            if (_dir == LiftDirection.縦) {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                stickobj.transform.localPosition = new Vector3(0, i, 0);
            }
            else if (_dir == LiftDirection.横) {
                this.transform.rotation = Quaternion.Euler(0, 0, 90);
                stickobj.transform.rotation = Quaternion.Euler(0, 0, 90);
                stickobj.transform.localPosition = new Vector3(0, -i, 0);
            }

            _list.Add(stickobj);
        }
    }

    private void CreateRide(int _stickIndex, List<GameObject> _list) {

        var fixIndex = _stickIndex - 1;
        var rideObj = Instantiate(_liftRideobj, this.transform.GetChild(0));

        if (fixIndex >= _list.Count || fixIndex < 0) {
            Debug.Log("リフトの乗る部分の位置指定が不正な値です。");
            _rideBlockIndex = 1;
            fixIndex = _rideBlockIndex - 1;
        }

        _nowRideBlock = fixIndex;
        rideObj.transform.position = _list[fixIndex].transform.position;
        rideObj.transform.localPosition += new Vector3(0, 0, -1);
    }

    //--------------------------------------------------------------
    // ・生成されたスティックの全削除
    // ・リフトの乗る部分のブロック削除
    //--------------------------------------------------------------

    [System.Obsolete]
    private void DestroyStickObject(List<GameObject> _list) {

        foreach (var obj in _list) {
            DestroyImmediate(obj.gameObject);
        }

        _list.Clear();

        // ダミーのオブジェクトを非アクティブにする
        _damiObj = this.transform.GetChild(0).Find("DamiObj").gameObject;

        // 実行したあとインスペクターから変更掛けた場合の削除処理
        var _deleteList = new List<GameObject>();
        for (int i = 0; i < this.transform.GetChild(0).childCount; i++) {
            var obj = this.transform.GetChild(0).GetChild(i).gameObject;
            if (_damiObj != obj) {
                _deleteList.Add(obj);
            }
        }

        foreach (var obj in _deleteList) {
            DestroyImmediate(obj.gameObject);
        }
    }

    private void OnEnable() {

        _nowRideBlock = _rideBlockIndex - 1;
        _rideObj = this.transform.GetChild(0).Find("Pf_LiftRideBlock(Clone)").gameObject;
        playerObj = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    public override void StickRotate(Vector3 center, Vector3 axis, int angle, Transform playerTransform) {

        var value = SceneManager.playerInput.FindAction("RotaionSelect").ReadValue<Vector2>();

        if (_liftDirection == LiftDirection.縦) {

            if (value.y > 0.5f) {
                Debug.Log("上昇");
                RideMove(Vector3.up, playerObj.transform);
            }
            else if (value.y < -0.8f) {
                Debug.Log("下昇");
                RideMove(Vector3.down, playerObj.transform);
            }
        }
        else if (_liftDirection == LiftDirection.横) {

            if (value.x > 0.5f) {
                Debug.Log("右移動");
                RideMove(Vector3.right, playerObj.transform);
            }
            else if (value.x < -0.8f) {
                Debug.Log("左移動");
                RideMove(Vector3.left, playerObj.transform);
            }
        }
    }

    private void RideMove(Vector3 offset, Transform playerTransform) {

        if (!_isMove) {

            bool succuse = false;

            if (offset == Vector3.up || offset == Vector3.right) {
                if (_nowRideBlock + 1 < _stickLength) {
                    _nowRideBlock++;
                    succuse = true;
                }
            }
            else if (offset == Vector3.down || offset == Vector3.left) {
                if (_nowRideBlock - 1 >= 0) {
                    _nowRideBlock--;
                    succuse = true;
                }
            }
        
            if (succuse) {
                playerTransform.SetParent(_rideObj.transform);
                _rideObj.transform.DOMove(offset, _liftSpeed).
                    SetRelative(true).
                    OnUpdate(() => { _isMove = true; }).
                    OnComplete(() => { _isMove = false; playerTransform.SetParent(null);});
            }
        }
    }
}

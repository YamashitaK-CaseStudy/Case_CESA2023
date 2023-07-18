using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SuzumuraTomoki;
using System;
using UnityEngine.InputSystem;
using Cinemachine;

public class Lift : RotatableObject {
    // リフトを伸ばす向き
    public enum LiftDirection {
        縦, 横,
    }

    [SerializeField] private LiftDirection _liftDirection;
    [SerializeField] private GameObject _liftStickVertickalobj;
    [SerializeField] private GameObject _liftStickHorizontalobj;
    [SerializeField] private GameObject _liftRideobj;
    [SerializeField] private GameObject _liftDameobj;
    [SerializeField] private GameObject _liftObjectobj;
    [SerializeField] private int _stickLength;
    public int _rideBlockIndex;
    public LiftSwich.LiftMove _liftMove;

    // リフトを構成してるパーツのオブジェクトの可変長配列
    private List<GameObject> _liftStickList = new List<GameObject>();
    private List<GameObject> _moveRotObjList = new List<GameObject>(); 
    private GameObject _rideObjs ,_stickObjs;
    private int _nowRideIndex = 0;
    private bool _isLiftUpdate = false;
    private Player _player;
    private CinemachineVirtualCamera _vCam;

    public GameObject GetRideBlocks {
        get { return _rideObjs; }
    }

    public GameObject GetCopyRideBlock {
        get { return _liftRideobj; }
    }

    public LiftSwich.LiftMove GetLiftMove {
        get { return _liftMove; }
    }

    public bool GetIsLiftUpdate {
        get { return _isLiftUpdate; }
    }

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

        // 棒をまとめる
        var liftSticks = new GameObject("LiftSticks");
        liftSticks.transform.parent = this.transform;
        liftSticks.transform.localPosition = Vector3.zero;
        liftSticks.transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (_dir == LiftDirection.縦) {

            // 棒の生成
            var stickMesh = Instantiate(_liftStickVertickalobj, liftSticks.transform);
            stickMesh.transform.localScale = new Vector3(1,_stickLength, 1);
            stickMesh.transform.localPosition = new Vector3(0, 0 + (0.5f * (_stickLength - 1)), 0);
        }
        else if(_dir == LiftDirection.横) {

            // 棒の生成
            var stickMesh = Instantiate(_liftStickHorizontalobj, liftSticks.transform);
            stickMesh.transform.localScale = new Vector3(_stickLength, 1, 1);
            stickMesh.transform.localPosition = new Vector3(0 + (0.5f * (_stickLength - 1)), 0 , 0);
        }

        for (int i = 0; i < length; i++) {

            var stickobj = new GameObject("stickobj");
            stickobj.transform.parent = liftSticks.transform;

            if (_dir == LiftDirection.縦) {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                stickobj.transform.localPosition = new Vector3(0, i, 0);
            }
            else if (_dir == LiftDirection.横) {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                stickobj.transform.rotation = Quaternion.Euler(0, 0, 90);
                stickobj.transform.localPosition = new Vector3(0, -i, 0);
            }

            _list.Add(stickobj);
        }
    }

    private void CreateRide(int _stickIndex, List<GameObject> _list) {

        // ブロックをまとめる
        var liftBlocks = new GameObject("LiftBlocks");
        liftBlocks.transform.parent = this.transform;
        liftBlocks.transform.localPosition = Vector3.zero;
        liftBlocks.transform.localRotation = Quaternion.Euler(0, 0, 0);
        _rideObjs = liftBlocks;

        var fixIndex = _stickIndex - 1;
        var rideObj = Instantiate(_liftRideobj, liftBlocks.transform);

        if (fixIndex >= _list.Count || fixIndex < 0) {
            Debug.Log("リフトの乗る部分の位置指定が不正な値です。");
            _rideBlockIndex = 1;
            fixIndex = _rideBlockIndex - 1;
        }

        _nowRideIndex = fixIndex;
        liftBlocks.transform.position = _list[fixIndex].transform.position;
        liftBlocks.transform.localPosition += new Vector3(0, 0, -1);
    }

    //--------------------------------------------------------------
    // ・生成されたスティックの全削除
    // ・リフトの乗る部分のブロック削除
    //--------------------------------------------------------------

    [System.Obsolete]
    private void DestroyStickObject(List<GameObject> _list) {

        _list.Clear();
        _liftDameobj.SetActive(false);

        var deleteList = new List<GameObject>();

        for(int i = 0;i < this.transform.childCount; i++) {

            var obj = this.transform.GetChild(i).gameObject;

            if(obj != _liftObjectobj && obj != _liftDameobj) {
                deleteList.Add(obj);
            }
        }

        foreach(var obj in deleteList) {
            DestroyImmediate(obj);
        }
    }

    public void AddMoveRotObj(GameObject obj) {

        if (!_moveRotObjList.Contains(obj)) {
            _moveRotObjList.Add(obj);
        }
    }

    public void ClearMoveRotObj() {

        foreach (var obj in _moveRotObjList) {
            var info = obj.GetComponent<LiftWithMoveInfo>();
            info.IsStopMove = false;
            info.LiftObj = null;
        }

        _moveRotObjList.Clear();
    }

    private void OnChildMoveRotObj() {
        foreach (var obj in _moveRotObjList) {
            obj.transform.parent = _rideObjs.transform;
        }
    }

    private void OffChildMoveRotObj() {
        foreach (var obj in _moveRotObjList) {
            obj.transform.parent = null;
        }
    }

    private bool IsStopMove() {

        bool stop = false;

        foreach (var obj in _moveRotObjList) {
            var Info = obj.GetComponent<LiftWithMoveInfo>();

            if (Info.IsStopMove) {
                stop = true;
                break;
            }
        }

        return stop;
    }

    private int GetStickLift(List<GameObject> _list,GameObject _stickobjs) {

        int num = 0;

        for(int i = 0;i < _stickObjs.transform.childCount; i++) {
            var child = _stickObjs.transform.GetChild(i).gameObject;

            if(child.name == "stickobj") {
                _list.Add(child);
            }
        }

        for (int i = 0; i < _stickObjs.transform.childCount; i++) {
            var child = _stickObjs.transform.GetChild(i).gameObject;

            if (_rideObjs.transform.localPosition.y == child.transform.localPosition.y) {
                num = (int)child.transform.localPosition.y;
                return num;
            }
        }

        return num;
    }

    private void LiftUp() {

        if (_nowRideIndex < _liftStickList.Count- 1 && !IsStopMove()) {

            _isLiftUpdate = true;

            _rideObjs.transform.DOMove(new Vector3(0, 1, 0), 1).SetRelative(true).OnComplete(LiftUp);

            foreach(var obj in _moveRotObjList) {
                obj.transform.DOMove(new Vector3(0, 1, 0), 1).SetRelative(true);
            }
           
            _nowRideIndex++;

            GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.Lift);
        }
       else  {
            Debug.Log("上昇終了" + _liftStickList.Count);
            _liftMove = LiftSwich.LiftMove.Down;
            _isLiftUpdate = false;
            _vCam.Priority = 0;
        }
    }

    private void LiftDown() {

        _isLiftUpdate = true;

        if (_nowRideIndex > 0 && !IsStopMove()) {
            _rideObjs.transform.DOMove(new Vector3(0, -1, 0), 1).SetRelative(true).OnComplete(LiftDown);

            foreach (var obj in _moveRotObjList) {
                obj.transform.DOMove(new Vector3(0, -1, 0), 1).SetRelative(true);
            }

            _nowRideIndex--;

            GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.Lift);
        }
        else  {
            Debug.Log("下降終了" + _nowRideIndex);
            _liftMove = LiftSwich.LiftMove.Up;
            _isLiftUpdate = false;
            _vCam.Priority = 0;
        }
    }

    [Obsolete]
    private void OnEnable() {

        _stickObjs = this.transform.FindChild("LiftSticks").gameObject;
        _rideObjs = this.transform.FindChild("LiftBlocks").gameObject;
        _nowRideIndex = GetStickLift(_liftStickList, _stickObjs);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.AddLift(this);
        _vCam = this.gameObject.GetComponent<LIftCamera>().GetVirtualCamera;
    }

    public void LiftAction(LiftSwich.LiftMove move) {

        if (move == LiftSwich.LiftMove.Up) {
            LiftUp();
        }

        else if (move == LiftSwich.LiftMove.Down) {
            LiftDown();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SuzumuraTomoki;
using System;

public class Lift : RotatableObject {
    // ���t�g��L�΂�����
    public enum LiftDirection {
        �c, ��,
    }

    [SerializeField] private LiftDirection _liftDirection;
    [SerializeField] private GameObject _liftStickobj;
    [SerializeField] private GameObject _liftRideobj;
    [SerializeField] private int _stickLength;
    public int _rideBlockIndex;

    // ���t�g���\�����Ă�p�[�c�̃I�u�W�F�N�g�̉ϒ��z��
    private List<GameObject> _liftStickList = new List<GameObject>();
    private GameObject _damiObj, _rideObj, playerObj;
    private float _liftSpeed = 0.5f;
    private int _nowRideBlock = 0;
    private bool _isMove = false;
 
    // �G�f�B�^�ɂ��X�V
    [System.Obsolete]
    public void ApplyInspector() {

        // ��U�S�v�f�폜����
        DestroyStickObject(_liftStickList);

        // �X�e�B�b�N�̐���
        CreateStick(_liftDirection, _stickLength, _liftStickList);

        // ���t�g�̏�镔���̐���
        CreateRide(_rideBlockIndex, _liftStickList);
    }

    private void CreateStick(in LiftDirection _dir, in int length, List<GameObject> _list) {

        _damiObj.SetActive(false);

        for (int i = 0; i < length; i++) {

            var stickobj = Instantiate(_liftStickobj, this.transform.GetChild(0));

            if (_dir == LiftDirection.�c) {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                stickobj.transform.localPosition = new Vector3(0, i, 0);
            }
            else if (_dir == LiftDirection.��) {
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
            Debug.Log("���t�g�̏�镔���̈ʒu�w�肪�s���Ȓl�ł��B");
            _rideBlockIndex = 1;
            fixIndex = _rideBlockIndex - 1;
        }

        _nowRideBlock = fixIndex;
        rideObj.transform.position = _list[fixIndex].transform.position;
        rideObj.transform.localPosition += new Vector3(0, 0, -1);
    }

    //--------------------------------------------------------------
    // �E�������ꂽ�X�e�B�b�N�̑S�폜
    // �E���t�g�̏�镔���̃u���b�N�폜
    //--------------------------------------------------------------

    [System.Obsolete]
    private void DestroyStickObject(List<GameObject> _list) {

        foreach (var obj in _list) {
            DestroyImmediate(obj.gameObject);
        }

        _list.Clear();

        // �_�~�[�̃I�u�W�F�N�g���A�N�e�B�u�ɂ���
        _damiObj = this.transform.GetChild(0).Find("DamiObj").gameObject;

        // ���s�������ƃC���X�y�N�^�[����ύX�|�����ꍇ�̍폜����
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

        if (_liftDirection == LiftDirection.�c) {

            if (value.y > 0.5f) {
                Debug.Log("�㏸");
                RideMove(Vector3.up, playerObj.transform);
            }
            else if (value.y < -0.8f) {
                Debug.Log("����");
                RideMove(Vector3.down, playerObj.transform);
            }
        }
        else if (_liftDirection == LiftDirection.��) {

            if (value.x > 0.5f) {
                Debug.Log("�E�ړ�");
                RideMove(Vector3.right, playerObj.transform);
            }
            else if (value.x < -0.8f) {
                Debug.Log("���ړ�");
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

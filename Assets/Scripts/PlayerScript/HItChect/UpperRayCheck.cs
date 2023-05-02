using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperRayCheck : MonoBehaviour {

    [SerializeField, Header("���̃��O�I�u�W�F�N�g��I������")]
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

        // ���̃��O�̈ʒu
        var _headpos = _headrigObj.transform.position;

        // �O����Ray���΂��ĕǂ̌��m������
        RaycastHit hitInfo;

        foreach(var myray in _oneRays) {

            // Ray�̃X�^�[�g�ʒu��ݒ�
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

    // ���i�N���X���V���A���C�Y
    [Serializable]
    class OneRay {
        // ray�̃X�^�[�g�n�_
        public Vector3 offsetPos;

        // rey�̒���
        public float distance;
    }
}

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

        // �O����Ray���΂��ĕǂ̌��m������
        RaycastHit hitInfo;

        // Ray�̃X�^�[�g�ʒu��ݒ�
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

    // ���i�N���X���V���A���C�Y
    [Serializable]
    class OneRay {
        // ray�̃X�^�[�g�n�_
        public Vector3 offsetPos;

        // rey�̒���
        public float distance;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour
{
    [SerializeField] GameObject _pf_SelectRange;
    [SerializeField] float _RangeMagnification = 1.0f;
    public GameObject _selectGameObject { get; set; } = null;
    private SphereCollider _collider;
    private float _SelectRangeRadius;
    private LineRenderer _linerendere;

    void StartObjectSelect() {

        // �X�P�[���ݒ�
        _pf_SelectRange.transform.localScale = new Vector3(0, 1, 1) * _RangeMagnification;

        // ���a�擾
        _SelectRangeRadius = _pf_SelectRange.transform.localScale.z;

        _linerendere = _pf_SelectRange.GetComponent<LineRenderer>();
        _linerendere.SetWidth(0.1f, 0.1f);
    }

    void UpdateObjectSelect() {

        var pos = this.gameObject.transform.position;

        // �͈̓I�u�W�F�N�g���v���C���[�̍��W�ɓ��ꑱ����
        _pf_SelectRange.gameObject.transform.position = new Vector3(pos.x, pos.y, 1);

        // �~�̒��ɓ����Ă�I���ł���I�u�W�F�N�g���X�g
        var inobjects = GetSelectRangeObjects(_SelectRangeRadius);

        // �~�̒��ɓ����Ă���I�u�W�F�N�g�����ԃv���C���[����߂��I�u�W�F�N�g���擾����
        var nearobject = GetInObjectNearObject(ref inobjects);

        //_linerendere.SetPosition(0, pos);
       
        if (nearobject != null) {
            _linerendere.SetPosition(1, nearobject.transform.position);
            _selectGameObject = nearobject;
           // Debug.Log(nearobject.name);
        }
        else {
//            _linerendere.SetPosition(1, pos);
            _selectGameObject = null;
           // Debug.Log(null);
        }
    }

    // �~�̒��ɓ����Ă�I�u�W�F�N�g���擾
    private List<GameObject> GetSelectRangeObjects(float _selectrangeradius) {

        // ���[���h���̉�]�ł���I�u�W�F�N�g���W�߂�
        GameObject[] rotateobjects = GameObject.FindGameObjectsWithTag("RotateObject");

        // �~�̒��ɓ����Ă�I�u�W�F�N�g���W�߂�
        List<GameObject> in_Objects = new List<GameObject>();
        foreach (var _object in rotateobjects) {

            // �v���C���[�Ƃ̋������v�Z
            var distance = Vector3.Distance(this.gameObject.transform.position, _object.transform.position);

            if (distance <= _SelectRangeRadius) {
                in_Objects.Add(_object);
            }
        }

        return in_Objects;
    }

    private GameObject GetInObjectNearObject(ref List<GameObject> list) {

        GameObject nearObject = null;
        float neardistance = 0.0f;

        // �v���C���[�����ԋ߂��I�u�W�F�N�g���擾
        for (int i = 0; i < list.Count; i++) {

            // �v���C���[�Ƃ̋������v�Z
            float distance = Vector3.Distance(this.gameObject.transform.position, list[i].transform.position);

            if (i == 0) {
                nearObject = list[i];
                neardistance = Math.Abs(distance);
            }
            else {
                if (Math.Abs(distance) < neardistance) {
                    nearObject = list[i];
                    neardistance = Math.Abs(distance);
                }
            }
        }

        return nearObject;
    }

}

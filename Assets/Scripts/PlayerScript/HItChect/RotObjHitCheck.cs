using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjHitCheck : MonoBehaviour{

    // �I�u�W�F�N�g�̏��
    private GameObject _rotObj = null;
    private GameObject _rotPartsObj = null;

    // ��]�I�u�W�F�N�g�ɐG��Ă邩�t���O
    private bool _isRotHit = false;

    // ����������]�I�u�W�F�N�g���ύX���ꂽ�Ƃ��t���O
    private bool _ischangeRotHit = false;

    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null)//�Ώ��Ö@�I�ł悭�Ȃ�
        {
            return;//���Ȃǂ͖���
        }
        if (other.transform.parent.parent == null)//�Ώ��Ö@�I�ł悭�Ȃ�
        {
            return;//���Ȃǂ͖���
        }
        // ��]�I�u�W�F�N�g�̎擾
        GetRotateObject(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {

        if(_rotPartsObj == null) {
            return;
        }

        if (other.gameObject != _rotPartsObj.gameObject) {
            return;
        }

        _rotObj = null;
        _rotPartsObj = null;
        _isRotHit = false;
    }

    public void InitChangeRotHit() {
        _ischangeRotHit = false;
    }

    // ��]�I�u�W�F�N�g�̎擾
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.parent.parent.gameObject.tag == "RotateObject") {

            _isRotHit = true;
            _ischangeRotHit = true;

            _rotObj = obj.transform.parent.parent.gameObject;
            _rotPartsObj = obj;
        }
        else {

            //_isRotHit = false;
        }
    }

    // Getter
    public  GameObject GetRotObj {
        get { return _rotObj; }
    }

    public GameObject GetRotPartsObj {
        get { return _rotPartsObj; }
    }

    public ref bool GetIsRotHit {
        get { return ref _isRotHit; }
    }

    public bool GetIsChangeRotHit {
        get {return _ischangeRotHit; }
    }

}

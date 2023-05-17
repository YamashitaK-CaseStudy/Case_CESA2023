using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotObjUnion : MonoBehaviour{

    static bool _isStartDoOnce = false;
    static RotObjUnionObtherber _obtherber = null;
    private RotatableObject _parentRotObjComp = null;
    private void Start() {

        if (!_isStartDoOnce) {
            _isStartDoOnce = true;
            _obtherber = GameObject.FindGameObjectWithTag("Observer").GetComponent<RotObjUnionObtherber>();
        }

        _parentRotObjComp = this.transform.root.gameObject.GetComponent<RotatableObject>();
    }

    private void OnTriggerEnter(Collider other) {
        // ��������RotateObject���擾
        GetRotateObject(other.gameObject);

        if(this.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
        if(_parentRotObjComp._isUnion)return;
        _parentRotObjComp._isUnion = true;
    }

    // ��]�I�u�W�F�N�g�̎擾
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.root.gameObject.tag == "RotateObject") {

            var rotcomp = obj.transform.root.gameObject.GetComponent<RotatableObject>();

            var _rotObj = obj.transform.root.gameObject;

            // �I�u�U�[�o�[�ɑf�ނ𑗂�
            _obtherber.AddunionMaterial(_rotObj);
            EffectGenerate(obj.gameObject);
        }
    }
}

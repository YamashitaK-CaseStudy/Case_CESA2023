using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjUnionObtherber : MonoBehaviour{

    // ��]�I�u�W�F�N�g�̍��̑f��
    private List<GameObject> _unionMaterials = new List<GameObject>();

    void Update() {

        if (_unionMaterials.Count < 2) {
            return;
        }

        // ���̃I�u�W�F�N�g�̑Ώۂł͖����I�u�W�F�N�g����菜��
        for (int i = _unionMaterials.Count - 1; i >= 0; i--) {
            if (_unionMaterials[i].GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) {
                _unionMaterials.RemoveAt(i);
            }
        }

        if (_unionMaterials.Count < 2) {
            return;
        }

        // ��]�I�u�W�F�N�g���~�܂��Ă邩�m�F
        foreach (var rotobj in _unionMaterials) {

            if (rotobj.GetComponent<RotatableObject>()._isRotating) {
                return;
            }
        }

        // ���̌��̉�]�I�u�W�F�N�g���擾����
        var basechildObject = _unionMaterials[0].transform.gameObject;

        // ��]�I�u�W�F�N�g�ɂԂ牺�����Ă�Object�I�u�W�F�N�g���擾����
        List<GameObject> objects = new List<GameObject>();
        for (int i = 1; i < _unionMaterials.Count; i++) {

            for(int j = 0; j < _unionMaterials[i].transform.childCount; j++) {
                objects.Add(_unionMaterials[i].transform.GetChild(j).gameObject);
            }

            // ��ɂȂ�����]�I�u�W�F�N�g���폜����
            Destroy(_unionMaterials[i].gameObject);
        }

        // �e��ύX
        foreach(var obj in objects) {
           obj.transform.parent = basechildObject.transform;
        }

        _unionMaterials.Clear();
    }

    // ���̑f�ނ̒ǉ�
    public void AddunionMaterial(GameObject material) {

        if(!_unionMaterials.Contains(material)){
            _unionMaterials.Add(material);
        }
    }
}

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
        var basechildObject = _unionMaterials[0].transform.GetChild(0).gameObject;

        for (int i = 1; i < _unionMaterials.Count; i++) {

            var childObject = _unionMaterials[i].transform.GetChild(0).gameObject;
            List<GameObject> parts = new List<GameObject>();

            // �p�[�c�I�u�W�F�N�g���W�߂�
            for (int j = 0; j < childObject.transform.childCount; j++) {
                parts.Add(childObject.transform.GetChild(j).gameObject);
            }

            // �p�[�c���ڂ��ւ���
            foreach (var part in parts) {
                part.transform.SetParent(basechildObject.transform);
                var pos = new Vector3(Mathf.Round(part.transform.localPosition.x), Mathf.Round(part.transform.localPosition.y),Mathf.Round(part.transform.localPosition.z));
                part.transform.localPosition = pos;
            }

            // ��ɂȂ�����]�I�u�W�F�N�g���폜����
            Destroy(_unionMaterials[i].gameObject); 
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

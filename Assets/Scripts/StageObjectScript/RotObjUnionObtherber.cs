using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjUnionObtherber : MonoBehaviour{

    // ��]�I�u�W�F�N�g�̍��̑f��
    private List<GameObject> _unionMaterials = new List<GameObject>();

    void Update() {

        if (_unionMaterials.Count == 0) {
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

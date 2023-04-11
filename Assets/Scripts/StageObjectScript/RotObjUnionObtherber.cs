using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjUnionObtherber : MonoBehaviour{

    // 回転オブジェクトの合体素材
    private List<GameObject> _unionMaterials = new List<GameObject>();

    void Update() {

        if (_unionMaterials.Count == 0) {
            return;
        }

        // 回転オブジェクトが止まってるか確認
        foreach (var rotobj in _unionMaterials) {

            if (rotobj.GetComponent<RotatableObject>()._isRotating) {
                return;
            }
        }

        // 合体元の回転オブジェクトを取得する
        var basechildObject = _unionMaterials[0].transform.GetChild(0).gameObject;

        for (int i = 1; i < _unionMaterials.Count; i++) {

            var childObject = _unionMaterials[i].transform.GetChild(0).gameObject;
            List<GameObject> parts = new List<GameObject>();

            // パーツオブジェクトを集める
            for (int j = 0; j < childObject.transform.childCount; j++) {
                parts.Add(childObject.transform.GetChild(j).gameObject);
            }

            // パーツを移し替える
            foreach (var part in parts) {
                part.transform.SetParent(basechildObject.transform);
            }

            // 空になった回転オブジェクトを削除する
            Destroy(_unionMaterials[i].gameObject); 
        }

        _unionMaterials.Clear();
    }

    // 合体素材の追加
    public void AddunionMaterial(GameObject material) {

        if(!_unionMaterials.Contains(material)){
            _unionMaterials.Add(material);
        }
    }
}

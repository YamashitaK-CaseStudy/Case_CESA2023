using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjUnionObtherber : MonoBehaviour{

    // 回転オブジェクトの合体素材
    private List<GameObject> _unionMaterials = new List<GameObject>();

    void Update() {

        if (_unionMaterials.Count < 2) {
            return;
        }

        // 合体オブジェクトの対象では無いオブジェクトを取り除く
        for (int i = _unionMaterials.Count - 1; i >= 0; i--) {
            if (_unionMaterials[i].GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) {
                _unionMaterials.RemoveAt(i);
            }
        }

        if (_unionMaterials.Count < 2) {
            return;
        }

        // 回転オブジェクトが止まってるか確認
        foreach (var rotobj in _unionMaterials) {

            if (rotobj.GetComponent<RotatableObject>()._isRotating) {
                return;
            }
        }

        // 合体元の回転オブジェクトの子供(Objects)を取得する
        var basechildObjects = _unionMaterials[0].transform.GetChild(0).gameObject;

        // 回転オブジェクトにぶら下がってるObjectオブジェクトを取得する
        List<GameObject> objects = new List<GameObject>();
        for (int i = 1; i < _unionMaterials.Count; i++) {

            for(int j = 0; j < _unionMaterials[i].transform.childCount; j++) {

                for (int k = 0; k < _unionMaterials[i].transform.GetChild(j).childCount; k++) {
                    objects.Add(_unionMaterials[i].transform.GetChild(j).GetChild(k).gameObject);
                }
            }

            // 空になった回転オブジェクトを削除する
            Destroy(_unionMaterials[i].gameObject);
        }

        // 親を変更
        foreach(var obj in objects) {
           obj.transform.parent = basechildObjects.transform;
        }

        // 合体した瞬間を
        Debug.Log("合体した");
        //basechildObjects.GetComponent<RotatableObject>().HitCheckFloorSettingStart();

        _unionMaterials.Clear();
    }

    // 合体素材の追加
    public void AddunionMaterial(GameObject material) {

        if(!_unionMaterials.Contains(material)){
            _unionMaterials.Add(material);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotObjUnion : MonoBehaviour{

    bool _isStartDoOnce = false;
    RotObjUnionObtherber _obtherber = null;
    private RotatableObject _parentRotObjComp = null;
    [SerializeField]private Collider _unionColliderX;
    [SerializeField]private Collider _unionColliderY;
    [SerializeField]private Collider _unionColliderZ;
    private void Start() {

        if (!_isStartDoOnce) {
            _isStartDoOnce = true;
            _obtherber = GameObject.FindGameObjectWithTag("Observer").GetComponent<RotObjUnionObtherber>();
        }

        _parentRotObjComp = this.transform.root.gameObject.GetComponent<RotatableObject>();
    }

    private void OnTriggerEnter(Collider other) {
        // ˝Á˝RotateObjectđćž
        GetRotateObject(other.gameObject);
        if(other.transform.root.gameObject.tag != "RotateObject") return;
        if(this.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
        var otherComp = other.transform.root.gameObject.transform.GetComponent<RotatableObject>();
        if(otherComp._isUnion)return;
        otherComp._isUnion = true;
    }

    // private void OnTriggerStay(Collider other) {
    //     if(other.transform.root.tag != "RotateObject") return;
    //     if(other.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
    //     // if(other.transform.root.GetComponent<RotatableObject>()._isRotating) return;
    //     Debug.Log(other.name);
    //     _parentRotObjComp.PreparationUinon();
    //     GetRotateObject(other.gameObject);
    //     Debug.Log("ĚüéĹ");
    // }

    // ń]IuWFNgĚćž
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.root.gameObject.tag == "RotateObject") {
            var rotcomp = obj.transform.root.gameObject.GetComponent<RotatableObject>();

            var _rotObj = obj.transform.root.gameObject;

            // IuU[o[ÉfŢđé
            _obtherber.AddunionMaterial(_rotObj);
            if(obj.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
            EffectGenerate(obj.gameObject);
            GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.Magnet);
        }
    }

    public void SetUnionCollider(bool flg){
        _unionColliderX.enabled = flg;
        _unionColliderY.enabled = flg;
        _unionColliderZ.enabled = flg;
    }
}

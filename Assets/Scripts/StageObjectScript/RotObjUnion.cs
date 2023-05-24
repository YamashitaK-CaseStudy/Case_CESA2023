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

    // private void OnTriggerEnter(Collider other) {
    //     if(other.transform.root.gameObject.tag != "RotateObject") return;
    //     if(this.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
    //     if(_parentRotObjComp._isUnion)return;
    //     _parentRotObjComp._isUnion = true;
    //     // ��������RotateObject���擾
    //     GetRotateObject(other.gameObject);
    // }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.transform.root.tag != "RotateObject") return;
        Debug.Log(other.name);
        if(other.gameObject.transform.root.gameObject.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
        GetRotateObject(other.gameObject);
        Debug.Log("���̏��������");
    }

    // ��]�I�u�W�F�N�g�̎擾
    private void GetRotateObject(GameObject obj) {

        if (obj.transform.root.gameObject.tag == "RotateObject") {
            var rotcomp = obj.transform.root.gameObject.GetComponent<RotatableObject>();

            var _rotObj = obj.transform.root.gameObject;

            // �I�u�U�[�o�[�ɑf�ނ𑗂�
            _obtherber.AddunionMaterial(_rotObj);
            EffectGenerate(obj.gameObject);
            GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.Magnet);
        }
    }

    public void SetUnionCollider(bool flg){
        _unionColliderX.enabled = flg;
        _unionColliderY.enabled = flg;
        _unionColliderZ.enabled = flg;
    }

    // ���̂��I���������Ƃ�m�点��
    public void FinishUnion(){

    }
}

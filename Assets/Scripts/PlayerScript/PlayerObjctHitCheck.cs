using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player {

    public List<GameObject> _hitRotObjectList;

    private GameObject _hitCollider;

    // Start is called before the first frame update
    void ObjcectHitCheckStart(){
        // �I�u�W�F�N�g�q�b�g����p�̎q�I�u�W�F�N�g���擾���Ƃ�
        _hitCollider = this.transform.Find("HitCollider").gameObject;
    }

    // Update is called once per frame
    void ObjectHitCheckUpdate(){
        
    }
}

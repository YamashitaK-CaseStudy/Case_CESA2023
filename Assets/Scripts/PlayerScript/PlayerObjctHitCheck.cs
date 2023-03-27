using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player {

    public List<GameObject> _hitRotObjectList;

    private GameObject _hitCollider;

    // Start is called before the first frame update
    void ObjcectHitCheckStart(){
        // オブジェクトヒット判定用の子オブジェクトを取得しとく
        _hitCollider = this.transform.Find("HitCollider").gameObject;
    }

    // Update is called once per frame
    void ObjectHitCheckUpdate(){
        
    }
}

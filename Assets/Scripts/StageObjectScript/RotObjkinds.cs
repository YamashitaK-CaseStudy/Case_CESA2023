using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjkinds : MonoBehaviour{

    // 回転の種類
    [field: SerializeField]
    public ObjectKind _RotObjKind { get; set; }

    // 回転オブジェクトの種類を列挙
    public enum ObjectKind {

        NomalRotObject,
        UnionRotObject,
        BoltRotObject,
        SpinObject
    }
}

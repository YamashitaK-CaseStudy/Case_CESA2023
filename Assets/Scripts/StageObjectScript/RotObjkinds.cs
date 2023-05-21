using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjkinds : MonoBehaviour{

    // ��]�̎��
    [field: SerializeField]
    public ObjectKind _RotObjKind { get; set; }

    // ��]�I�u�W�F�N�g�̎�ނ��
    public enum ObjectKind {

        NomalRotObject,
        UnionRotObject,
        BoltRotObject,
        SpinObject
    }
}

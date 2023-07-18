using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    [SerializeField] public GameObject _trackingObj = null;
    [SerializeField] public Vector3 _offset;


    private Transform _trackingObjTransform;

    // Start is called before the first frame update
    void Start(){
        if (_trackingObj == null) {
            Debug.Log("í«ê’ëŒè€Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
        }


        _trackingObjTransform = _trackingObj.transform;

    }

    // Update is called once per frame
    void Update(){
        this.transform.position = _trackingObjTransform.position + _offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    [SerializeField] private GameObject _TrackingObjct;
    [SerializeField] private Vector3 _Offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _TrackingObjct.transform.position+_Offset;
    }
}

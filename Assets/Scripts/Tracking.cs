using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{

    [SerializeField] public GameObject _TrackingObj;
    [SerializeField] public Vector3 _Offset; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _TrackingObj.transform.position;
    }
}

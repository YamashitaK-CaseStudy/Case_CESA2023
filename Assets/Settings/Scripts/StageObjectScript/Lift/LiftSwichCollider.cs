using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSwichCollider : MonoBehaviour
{
    private LiftSwich _liftswich;
    private Lift _lift;

    // Start is called before the first frame update
    void Start()
    {
        _liftswich = this.transform.root.GetComponent<LiftSwich>();
        _lift = this.transform.root.GetComponent<LiftSwich>().GetLift;
    }

    // Update is called once per frame
    void Update()
    {
        _doOnce = false;
    }

    private bool _doOnce = false;

    [System.Obsolete]
    private void OnTriggerEnter(Collider other) {

        var hitObj = other.transform.root.gameObject;

        if(hitObj.tag == "RotateObject" && !_doOnce && !_lift.GetIsLiftUpdate) {

            Debug.Log("回転オブジェクト接触");

            _liftswich.AllRotObjColliderChnage(_lift.GetLiftMove);
            _lift.ClearMoveRotObj();
            _liftswich.StartCoroutine(_liftswich.WaitProces());

            _doOnce = true;
        }
    }
}

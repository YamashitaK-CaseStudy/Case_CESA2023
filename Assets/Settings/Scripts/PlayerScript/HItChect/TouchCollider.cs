using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCollider : MonoBehaviour
{

    GameObject _parent = null; 

    GameObject _touchObject = null; 

    // Start is called before the first frame update
    void Start(){
        // ���g�̐e���擾
        _parent = this.transform.parent.gameObject;

        if ( _parent.tag != "Player" ) {
            Debug.Log("Parent object is invalid.");
        }

        if ( _parent = null ) {
            Debug.Log("Parent object is invalid.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( _touchObject != null ) {
            //Debug.Log(_touchObject.name);
        }
    }

    public GameObject GetTouchObject() {
        Debug.Log("Get the touched object ");

        if ( _touchObject == null ) {
            return null;
        }

        return _touchObject;
    }

    void OnTriggerEnter(Collider t) {

        //            This                Objrct BaceObj
        var obj  = t.gameObject.transform.parent.parent.gameObject;

        if ( obj.tag == "RotateObject" ) {
            Debug.Log("Touched.");
            Debug.Log(obj.name);
            _touchObject = obj;
        }
    }

	private void OnTriggerExit(Collider t) {

        if ( _touchObject != null ) {
            Debug.Log("leaving.");
            Debug.Log(_touchObject.name);
            _touchObject = null;
        }
	}
}

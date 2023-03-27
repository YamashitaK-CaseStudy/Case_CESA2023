using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCollider : MonoBehaviour
{

    GameObject _parent = null; // ���g�̐e�I�u�W�F�N�g�@

    GameObject _touchObject = null; // �G��Ă���I�u�W�F�N�g

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
            Debug.Log(_touchObject.name);
        }
    }

    public GameObject GetTouchObject() {
        Debug.Log("Get the object touched");

        if ( _touchObject == null ) {
            return null;
        }

        return _touchObject;
    }

    void OnTriggerEnter(Collider t) {

        var obj  = t.gameObject.transform.parent.transform.parent.gameObject;

        Debug.Log(obj.name);

        if ( obj.tag == "RotateObject" ) {
            Debug.Log("Touched.");
            _touchObject = obj;
        }
    }

	private void OnTriggerExit(Collider t) {

        if ( _touchObject != null ) {
            _touchObject = null;
            Debug.Log("leaving.");
        }
	}
}

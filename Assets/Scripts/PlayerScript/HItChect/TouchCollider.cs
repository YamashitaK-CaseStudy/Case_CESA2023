using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCollider : MonoBehaviour
{

    GameObject _parent = null; // 自身の親オブジェクト　

    GameObject _touchObject = null; // 触れているオブジェクト

    // Start is called before the first frame update
    void Start(){
        // 自身の親を取得
        _parent = this.transform.parent.gameObject;

        if ( _parent.tag != "Player" ) {
            Debug.Log("親オブジェクトが不正です");
        }

        if ( _parent = null ) {
            Debug.Log("親オブジェクトが不正です");
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
        Debug.Log("呼ばれた");

        if ( _touchObject == null ) {
            return null;
        }

        return _touchObject;
    }

    void OnTriggerEnter(Collider t) {

        var obj  = t.gameObject.transform.parent.transform.parent.gameObject;

        Debug.Log(obj.name);

        if ( obj.tag == "RotateObject" ) {
            Debug.Log("触れた");
            _touchObject = obj;
        }
    }

	private void OnTriggerExit(Collider t) {

        if ( _touchObject != null ) {
            _touchObject = null;
            Debug.Log("離れた");
        }
	}
}

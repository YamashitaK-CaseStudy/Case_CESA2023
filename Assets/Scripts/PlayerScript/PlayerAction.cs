using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{
    // Start is called before the first frame update
    void StartAction(){
        
    }

    // Update is called once per frame
    void UpdateAction(){

        if ( Input.GetButtonDown("RotateSelfAxis") ) {
            Debug.Log("その場でまわす小アクション！");

            if ( _selectGameObject == null) {
                return;
            }

            // まわす小を起動する関数を呼び出す
            _selectGameObject.GetComponent<RotatableObject>().StartRotate();
   
        }

        if ( Input.GetButtonDown("RotateExturnAxis") ) {
            Debug.Log("その場でまわす大アクション！");

            if ( _selectGameObject == null ) {
                return;
            }

            // まわす小を起動する関数を呼び出す
            _selectGameObject.GetComponent<RotatableObject>().StartSpin();
        }

    }

}

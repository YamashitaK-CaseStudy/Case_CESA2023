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
            Debug.Log("���̏�ŉ�]������A�N�V�����I");

            if ( _selectGameObject == null) {
                return;
            }

            // ��]�֐����Ăяo��
            _selectGameObject.GetComponent<RotatableObject>().SpinAxisSelf();
   
        }

        if ( Input.GetButtonDown("RotateExturnAxis") ) {
            Debug.Log("���̏�ŉ�]������A�N�V�����I");

            if ( _selectGameObject == null ) {
                return;
            }

            // ��]�֐����Ăяo��
            _selectGameObject.GetComponent<RotatableObject>().RotateSmallAxisExtern(this.transform.position);

        }

    }

}

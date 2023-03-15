using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    // 乗っ取りフラグ
    private bool _isPossession { get; set; } = false;

    void StartPossession(){
    }

    void UpdatePossession(){

        if (Input.GetButtonDown("Possession")) {
            if (_isPossession == false) {
                OnPossession();
            }
            else {
                OffPossession();
            }
        }

    }

    private void OnPossession() {
       
        if(_selectGameObject != null) {
            _isPossession = true;

            // 特定の回転を行う
            _selectGameObject.GetComponent<MeshRenderer>().material.color = Color.red;

            // マテリアルの色を変更する
            this.GetComponent<MeshRenderer>().material.color = Color.gray;


            _selectGameObject.GetComponent<RotatableObject>().RotateSmallAxisSelf();

        }
    }

    private void OffPossession() {
        _isPossession = false;

        // マテリアルの色を変更する
        _selectGameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
        this.GetComponent<MeshRenderer>().material.color = Color.red;

    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.name == "RArm" || other.transform.gameObject.name == "LArm") {
            transform.SetParent(other.transform);

            Debug.Log("のてない");
        }
    }

    private void OnTriggerExit(Collider other) {
        transform.SetParent(null);
    }
}

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
        }
    }

    private void OffPossession() {
        _isPossession = false;

        // マテリアルの色を変更する
        _selectGameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
        this.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void OnTriggerEnter(Collider other) {
        if (transform.gameObject.name == "RArm") {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit(Collider other) {
        transform.SetParent(null);
    }
}

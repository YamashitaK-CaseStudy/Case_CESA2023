using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    // èÊÇ¡éÊÇËÉtÉâÉO
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
        _isPossession = true;
    }

    private void OffPossession() {
        _isPossession = false;
    }
}

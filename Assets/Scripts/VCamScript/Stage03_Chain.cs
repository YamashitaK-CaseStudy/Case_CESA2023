using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Stage03_Chain : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera _VCam;
    private void OnTriggerStay(Collider other) {
        if(other.transform.root.tag != "Player") return;
        _VCam.Priority = 11;
    }

    private void OnTriggerExit(Collider other) {
        if(other.transform.root.tag != "Player") return;
        _VCam.Priority = 9;
    }
}

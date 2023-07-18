using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Stage03_Chain : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera _VCam;
    [SerializeField] GameObject _player;
    [SerializeField] float xmin,xmax,ymin = 0,ymax = 20;
    Transform _playerTrans;
    private void Start(){
        _playerTrans = _player.transform;
    }

    private void Update() {
        if(_playerTrans.position.x > xmin && _playerTrans.position.x < xmax){
            if(_playerTrans.position.y > ymin && _playerTrans.position.y < ymax){
                _VCam.Priority = 11;
            }else{
                _VCam.Priority = 9;
            }
        }else{
            _VCam.Priority = 9;
        }
    }
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.transform.root.tag != "Player") return;
    //     _VCam.Priority = 11;
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.transform.root.tag != "Player") return;
    //     _VCam.Priority = 9;
    // }
}
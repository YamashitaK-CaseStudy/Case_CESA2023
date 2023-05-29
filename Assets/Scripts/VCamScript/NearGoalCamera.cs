using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NearGoalCamera : MonoBehaviour
{
    private const int OFF = 9;
    private const int ON = 11;

    [SerializeField] private float _switchPosHorizontal;
    [SerializeField] private CinemachineVirtualCamera _ResultVirtualCamera;
    [SerializeField] private GameObject _player;

    private void Update()
    {
        if (_ResultVirtualCamera.Priority == OFF)
        {
            if (_player.transform.position.x > _switchPosHorizontal)
            {
                _ResultVirtualCamera.Priority = ON;
            }
        }
        else
        {
            if (_player.transform.position.x < _switchPosHorizontal)
            {
                _ResultVirtualCamera.Priority = OFF;
            }
        }
    }
}

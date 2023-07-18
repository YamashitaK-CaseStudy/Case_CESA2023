using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingFPS : MonoBehaviour
{
    [SerializeField] private int _settingFps;

    void Start() {

        Application.targetFrameRate = _settingFps;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultInputControl : MonoBehaviour
{
    private void Awake()
    {
        Fader.stopInput = _inputs.FindActionMap("UI");
    }


    [SerializeField]private UnityEngine.InputSystem.InputActionAsset _inputs;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultInputControl : MonoBehaviour
{
    private void Awake()
    {
        Fader.stopInput = _inputs.FindActionMap("UI");
    }

    [SerializeField] UnityEngine.InputSystem.InputActionAsset _inputs = null;
}

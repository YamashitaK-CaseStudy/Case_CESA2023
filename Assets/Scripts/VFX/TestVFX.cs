using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestVFX : MonoBehaviour
{
    [SerializeField] private VisualEffect effect;
    // Start is called before the first frame update
    void Start()
    {
        effect.SendEvent("OnStop");
    }

    // Update is called once per frame
    void Update()
    {
        if(effect == null) return;
        if(Input.GetKey(KeyCode.DownArrow)){
            effect.SendEvent("OnPlay");
        }
        if(Input.GetKey(KeyCode.UpArrow)){
             effect.SendEvent("OnStop");
        }
    }
}

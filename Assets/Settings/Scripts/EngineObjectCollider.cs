using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineObjectCollider : MonoBehaviour
{
    // Start is called before the first frame update
    EngineObject _parentEngine = null;
    void Start()
    {
        _parentEngine = this.transform.root.gameObject.GetComponent<EngineObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.root.gameObject.tag != "RotateObject") return;
        var comp = other.transform.root.gameObject.GetComponent<RotatableObject>();
        var axis = comp._rotAxis;
        if(comp._oldAngle <= 0) axis *= -1;
        _parentEngine.StartUpEngine(axis);
        comp.SetisHitFloor();
    }
}

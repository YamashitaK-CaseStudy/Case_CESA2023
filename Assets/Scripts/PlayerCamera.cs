using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour{

    // オフセット
    [SerializeField] public Vector3 cameraOffset = new Vector3(0.0f,0.0f,0.0f);

    private Transform target;
    private string targetName = "Player";

    // Start is called before the first frame update
    void Start(){
        target = GameObject.Find(targetName).transform;
    }

    // Update is called once per frame
    void LateUpdate(){
        this.transform.position = target.TransformPoint(cameraOffset);
        this.transform.LookAt(target);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour{

    public Vector3 offsetRotAxis;

    virtual public void StickRotate(Vector3 center, Vector3 axis, int angle, Transform playerTransform) {

        if(angle == 0) {
            return;
        }

        if (angle < 0) {
            offsetRotAxis = new Vector3(0, -1, 0);
        }
        else {
            offsetRotAxis = new Vector3(0, 1, 0);
        }

        StartRotate(center, axis, angle, playerTransform);
        PlayPartical();
    }


}

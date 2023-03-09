using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour
{
    private PlayerInput _input;

    private Vector3 GetRightStick() {
        var axisx = _input.actions["AngleSelect"].ReadValue<Vector2>().x;
        var axisy = _input.actions["AngleSelect"].ReadValue<Vector2>().y;
        return new Vector3(axisx, axisy, 0);
    }

    private void Awake() {
        TryGetComponent(out _input);
    }

    void StartObjectSelect() {
    }

    void UpdateObjectSelect() {
        var pos = this.gameObject.transform.position;

        Ray ray = new Ray(pos, GetRightStick() * 100.0f);


        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.collider.gameObject.name);
        }

        Debug.DrawRay(ray.origin, ray.direction * 100.0f,Color.red);
      //  Debug.Log(GetRightStick());
    }
}

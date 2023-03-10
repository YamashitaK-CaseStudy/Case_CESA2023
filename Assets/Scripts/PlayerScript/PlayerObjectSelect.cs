using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour
{
    private PlayerInput _input;
    private LineRenderer _linerenderer;
    public GameObject _selectGameObject { get; set; } = null;

    private Vector3 GetRightStick() {
        var axisx = _input.actions["AngleSelect"].ReadValue<Vector2>().x;
        var axisy = _input.actions["AngleSelect"].ReadValue<Vector2>().y;
        return new Vector3(axisx, axisy, 0);
    }

    [System.Obsolete]
    private void Awake() {
        TryGetComponent(out _input);
        _linerenderer = gameObject.AddComponent<LineRenderer>();

        _linerenderer.SetWidth(0.1f,0.1f);
        _linerenderer.SetColors(Color.blue, Color.blue);
        _linerenderer.material.color = Color.blue;
    }

    void StartObjectSelect() {
    }

    void UpdateObjectSelect() {
        var pos = this.gameObject.transform.position;

        Ray ray = new Ray(pos, GetRightStick() * 100.0f);
        _linerenderer.SetPosition(0, pos);
     
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {

            _linerenderer.SetPosition(1, hit.point);
            _selectGameObject = hit.collider.gameObject;
        }
        else {
            _linerenderer.SetPosition(1, pos + GetRightStick() * 100.0f);
            _selectGameObject = null;
        }

        if(_selectGameObject != null) {
            Debug.Log(_selectGameObject.name);
        }
        else {
            Debug.Log("NULL");
        }

        //Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red);
        //Debug.Log(GetRightStick());
    }
}

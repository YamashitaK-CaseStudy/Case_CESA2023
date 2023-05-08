using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour{

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private TimeMeasurement _timeMeasurement;
      
    // Start is called before the first frame update
    void Start(){

        StartMove();
        PlayerBlockColliderStart();
        PlayerRotationStart();
        PlayerSkAnimationStart();

    }

    void Update(){

        UpdateMove();
        PlayerBlockColliderUpdate();
        PlayerRotationUpdate();
        PlayerSkAnimationUpdate();

    }
}

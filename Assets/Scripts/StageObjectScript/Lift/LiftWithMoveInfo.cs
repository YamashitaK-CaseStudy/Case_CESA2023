using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftWithMoveInfo : MonoBehaviour{

    private GameObject _LiftObj = null;
    private bool _isStopMove = false;

    public bool IsStopMove {

        set { _isStopMove = value; }
        get { return _isStopMove; }
    }

    public GameObject LiftObj {

        set { _LiftObj = value; }
        get { return _LiftObj; }
    }
}

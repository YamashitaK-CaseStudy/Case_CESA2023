using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public partial class Player : MonoBehaviour{

    [SerializeField] private Animator _skAnimator;

    // Start is called before the first frame update
    void Start(){

        StartMove();
        StartAction();
        //StartWarp();
        //StartPossession();

    }

    void Update(){

        //// œßˆË‚µ‚Ä‚È‚¢Žž
        //if (_isPossession == false) {
          
        //}
        UpdateMove();
        UpdateAction();
        //UpdateWarp();
        //UpdatePossession();
    }
}

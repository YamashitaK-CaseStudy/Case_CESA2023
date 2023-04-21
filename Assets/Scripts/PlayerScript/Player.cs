using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){

        StartMove();
        StartAction();
        //StartWarp();
        //StartPossession();

    }

    // Update is called once per frame
    void Update()
    {
        //// œßˆË‚µ‚Ä‚È‚¢Žž
        //if (_isPossession == false) {
          
        //}
        UpdateMove();
        UpdateAction();
        //UpdateWarp();
        //UpdatePossession();
    }
}

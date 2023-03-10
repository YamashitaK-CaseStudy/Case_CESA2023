using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){

        //Debug.Log("start");

        StartMove();
        //StartWarp();
        StartAction();
        StartObjectSelect();

        //Debug.Log("end");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
        //UpdateWarp();
        UpdateAction();
        UpdateObjectSelect();
    }
}

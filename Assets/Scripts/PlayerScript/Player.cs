using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){

        StartMove();
        //StartWarp();
        StartAction();
        StartObjectSelect();
        StartPossession();

    }

    // Update is called once per frame
    void Update()
    {
        // �߈˂��ĂȂ���
        if (_isPossession) {
            UpdateMove();
            //UpdateWarp();
            UpdateAction();
        }
      
        UpdateObjectSelect();
  UpdatePossession();
    }
}

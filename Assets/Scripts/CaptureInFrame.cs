using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureInFrame : MonoBehaviour
{
    [SerializeField]private GameObject _Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var nowPlayerPos = _Player.transform.position;

        var newPos = this.transform.position;
        newPos.x = nowPlayerPos.x;

        this.transform.position = newPos;
    }
}

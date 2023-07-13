using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spininig : MonoBehaviour
{
    [SerializeField] private float _spinRateSec = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(new Vector3(0,_spinRateSec,0) * Time.deltaTime);
    }
}

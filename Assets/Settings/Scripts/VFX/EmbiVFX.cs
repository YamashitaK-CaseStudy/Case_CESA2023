using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EmbiVFX : MonoBehaviour
{
    [SerializeField] public GameObject _GoalObj;
    void Start()
    {
        var pos = _GoalObj.transform.position;
        pos.x = pos.x / 2.0f;
        pos.y = 0.0f;
        this.transform.position = pos;
        for(int i = 0; i < this.transform.childCount; i++){
            GameObject tmp = this.transform.GetChild(i).gameObject;
            tmp.SetActive(true);
        }
    }
}

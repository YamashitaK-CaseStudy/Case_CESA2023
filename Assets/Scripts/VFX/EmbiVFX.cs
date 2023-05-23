using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EmbiVFX : MonoBehaviour
{
    [SerializeField] public GameObject _GoalObj;
    void Start()
    {
        this.transform.position = _GoalObj.transform.position / 2;
        _GoalObj = null;
        for(int i = 0; i < this.transform.childCount; i++){
            GameObject tmp = this.transform.GetChild(i).gameObject;
            tmp.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [SerializeField] private int HP;

    private void StartHP(){
        
    }
    private void UpdateHP(){

    }

    public void Damage(){
        HP -= 1;
        Debug.Log(HP);
    }

    public int GetHP(){
        return HP;
    }
}

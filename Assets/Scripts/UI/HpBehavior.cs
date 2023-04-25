using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBehavior : MonoBehaviour
{
    private void Start()
    {
        _hp = _MaxHp;
    }

    public void Decrease(int value)
    {
        _hp -= value;
    }

    [SerializeField] private int _MaxHp = 5;
    private int _hp;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiNumberBehavior : MonoBehaviour
{

    public int Number
    {
        set
        {
            if (value < 0 || value > 9)
            {
                return;
            }
            GetComponent<UnityEngine.UI.Image>().sprite = _spriteArray[value];
        }
    }

    [SerializeField] private Sprite[] _spriteArray;
}

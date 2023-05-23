using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHP : MonoBehaviour
{
    [SerializeField]private Sprite[] _spriteListHP;

    public void ChangeIcon(int remainHP) {
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = _spriteListHP[remainHP - 1];
    }
}

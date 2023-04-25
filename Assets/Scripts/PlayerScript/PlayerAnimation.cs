using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Player _playerScript;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _playerScript = transform.root.gameObject.GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     
        //if(Mathf.Abs( _playerScript.moveVelocity.x) <= 0) {
        //    _animator.SetBool("Walk", false);
        //}
        //else {
        //    _animator.SetBool("Walk", true);
        //}
    }
}

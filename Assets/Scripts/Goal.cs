using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetButtonDown("Jump")) {
            SuzumuraTomoki.SceneManager.Instance.LoadResult();
       }
    }

    
	private void OnCollisionEnter(Collision other) {
        Debug.Log("へい");
    }

}

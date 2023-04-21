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
            sceneManager.LoadResult();
       }
    }

    
	private void OnCollisionEnter(Collision other) {
        Debug.Log("へい");
    }

    [SerializeField] private SuzumuraTomoki.SceneManager sceneManager;
}

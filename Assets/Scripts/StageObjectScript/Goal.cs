using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
	private void OnTriggerEnter(Collider other) {
        if(other.gameObject.transform.root.gameObject.tag != "Player") return;
        SuzumuraTomoki.SceneManager.instance.LoadResult();
    }

	public void Update(){
		if (Input.GetKeyDown(KeyCode.R))
		{
			SuzumuraTomoki.SceneManager.instance.LoadCurrentScene();
		}
    }

}

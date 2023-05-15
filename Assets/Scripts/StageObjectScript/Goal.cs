using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
	private void OnTriggerEnter(Collider other) {
        if(other.transform.root.gameObject.tag != "Player") return;

		SuzumuraTomoki.SceneManager.LoadResult();
    }

	public void Update(){
		if (Input.GetKeyDown(KeyCode.R))
		{
			SuzumuraTomoki.SceneManager.LoadCurrentScene();
		}
    }

}

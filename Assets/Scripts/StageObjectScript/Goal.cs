using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
	private void OnTriggerEnter(Collider other) {
        if(other.transform.root.gameObject.tag != "Player") return;

        SuzumuraTomoki.SceneManager.missionClear = true;
        SuzumuraTomoki.SceneManager.LoadResult();
    }

}

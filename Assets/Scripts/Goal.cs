using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
	private void OnTriggerEnter(Collider other) {
        sceneManager.LoadResult();
    }

    [SerializeField] private SuzumuraTomoki.SceneManager sceneManager;
}

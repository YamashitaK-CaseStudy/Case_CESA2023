using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(Time.timeScale == 0) return;

		Vector3 pos = transform.position;
		transform.position = new Vector3(pos.x + 0.5f, pos.y, pos.z);
	}
}

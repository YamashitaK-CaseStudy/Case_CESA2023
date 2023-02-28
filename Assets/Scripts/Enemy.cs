using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameCharacter
{
	[SerializeField] int waitTime_Dir;

	// Start is called before the first frame update
	void Start()
	{
		Debug.Log("Enemy");
	}

	// Update is called once per frame
	void Update()
	{
		if(CheckPause()) return;
	}

	void Move(){
		float ratio = this.speed / 100f;
		float x = (transform.position.x + 1.0f * ratio) * (int)dir;

	}

	void Dead(){

	}
}

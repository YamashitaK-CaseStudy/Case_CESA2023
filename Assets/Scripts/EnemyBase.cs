using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyBase : GameCharacter
{
	[SerializeField] int waitTime_Dir;

	virtual protected void Move(){

	}

	protected void Dead(float time = 0.0f){
		// インスタンスの放棄
		Destroy(this.gameObject,time);
	}

	protected void ChangeDir(){
		dir = (Direction)((int)dir * -1);
	}
}

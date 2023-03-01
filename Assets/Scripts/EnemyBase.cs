using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyBase : GameCharacter
{
	[SerializeField] int waitTime_Dir;

	virtual protected void Move(){

	}

	protected void Dead(){
		// インスタンスの放棄
		Destroy(this);
	}

	protected void ChangeDir(){
		dir = (Direction)((int)dir * -1);
	}
}

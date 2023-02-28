using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

abstract public class GameCharacter : MonoBehaviour
{
	protected enum Direction{
		Right = 1,
		Left = -1
	}
	// 変数
	[SerializeField] protected Direction dir;
	[SerializeField] protected int HP;
	[SerializeField] protected int ATK;
	[SerializeField] protected int speed;
	protected bool isDead = true;
	/// <summary>
	/// 現在ポーズされているかどうかの判定
	/// </summary>
	/// <returns>Yes:true No:false</returns>
	protected bool CheckPause(){
		bool result = false;
		if(Time.timeScale == 0) result = true;
		return result;
	}
	/// <summary>
	/// 現在ポーズされているかどうかの判定
	/// </summary>
	/// <param name = "damage">ダメージ量</param>
	protected void Damage(int damage){
		HP -= damage;
		if(HP < 0) HP = 0;
	}
	/// <summary>
	/// このキャラクタが死んでるかどうか
	/// </summary>
	/// <returns>Yes:true No:false</returns>
	protected bool CheckDead(){
		if(HP == 0) isDead = true;
		return isDead;
	}
}

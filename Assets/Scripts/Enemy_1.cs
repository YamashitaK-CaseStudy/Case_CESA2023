using UnityEngine;

public class Enemy_1 : EnemyBase
{
	
	// Start is called before the first frame update
	void Start(){
	}

	// Update is called once per frame
	void Update()
	{
		// ポーズ状態かどうか確認
		//if(CheckPause()) return;

		// 移動処理
		//Move();

		// 死んでるかどうか確認
		//if(CheckDead()) Dead();
	}

	private void OnTriggerEnter(Collider other) {

		// プレイヤーと接触時
		if (other.gameObject.tag == "Player") {

			// 当たったら死ぬ
			Dead();
		}
	}

	private void OnCollisionEnter(Collision other) {

		// 壁と接触時
		if (other.gameObject.tag == "Wall") {

			// 壁と接触したら方向を転換する
			ChangeDir();
		}
	}

	protected override void Move(){

		//Debug.Log("move");
		// 移動速度の割合を計算
		//float ratio = this.speed / 100f;
		// 格納座標の計算
		//float x = (transform.position.x + 3.0f * ratio * Time.deltaTime) * (int)dir;
		//float y = transform.position.y;	// 変更予定ないのでそのまま
		//float z = transform.position.z;	// 変更予定ないのでそのまま
		// 計算した座標を格納
		//transform.position = new Vector3(x,y,z);

		// 一秒間での移動量
		float offset_x = speed * (int)dir * Time.deltaTime;

		//計算した座標を格納
		transform.Translate(offset_x, 0, 0);
	}
}

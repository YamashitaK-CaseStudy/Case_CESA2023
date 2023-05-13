using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public partial class RotatableObject : MonoBehaviour
{

	private Transform _playerTransform = null;// プレイヤーのトランスフォーム
	private float _oldAngle = 0.0f;
	private float _polatAngle = 0.0f;
	private bool _isSpin;
	private Quaternion _oldRotAngle;
	private Vector3 _oldPos;
	public void StartRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle)
	{
		if (_isSpin || _isRotating)
		{
			return;
		}
		// 回転の中心を設定
		_axisCenterWorldPos = rotCenter;
		// 回転軸を設定
		_rotAxis = rotAxis;
		// 回転オフセット値をセット
		_angle = rotAngle;
		// フラグを立てる
		_isRotating = true;

		// 角度による補正値を計算する
		_polatAngle = _angle / 90;

		// 回転前の角度を持っておく
		_oldRotAngle = this.transform.rotation;
		_oldPos = this.transform.position;

		// 床と連鎖の当たり判定を行う
		SetChildHitCheckFloorFlg(true);
		SetChildHitCheckChainFlg(true);
		// めり込み判定を切っておく
		SetChildHitCheckInto(false);

		// 経過時間を初期化
		_elapsedTime = 0.0f;
		// トレイルの起動
		PlayPartical();
	}


	public void StartRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle, Transform playerTransform)
	{
		if (_isSpin || _isRotating)
		{
			return;
		}
		Debug.Log("プレイヤー回転");
		StartRotate(rotCenter, rotAxis, rotAngle);
		// トランスフォームを格納
		_playerTransform = playerTransform;
	}

	public void StartRotateReflect(Vector3 rotCenter, Vector3 rotAxis, int rotAngle)
	{
		if (_isSpin || _isRotating)
		{
			return;
		}
		Debug.Log("反射回転");
		StartRotate(rotCenter, rotAxis, rotAngle);
		SetChildHitCheckFloorFlg(false);
	}

	public void StartRotateChain(Vector3 rotCenter, Vector3 rotAxis, int rotAngle)
	{
		if (_isSpin || _isRotating)
		{
			return;
		}
		Debug.Log("連鎖回転");
		StartRotate(rotCenter, rotAxis, rotAngle);
	}

	// まわす小の更新
	protected void UpdateRotate()
	{
		if (_doOnce)
		{
			_isRotateStartFream = false;
		}

		// 回転中かフラグ
		if (_isRotating)
		{
			if (!_doOnce)
			{
				_isRotateStartFream = true;
				_doOnce = true;
			}
			// リクエストデルタタイムを求める
			// リクエストデルタタイム：デルタタイムを1回転に必要な時間で割った値
			// これの合算値が1になった時,1回転に必要な時間が経過したことになる
			float requiredDeltaTime = Time.deltaTime / (_rotRequirdTime * Math.Abs(_polatAngle));
			_elapsedTime += requiredDeltaTime;

			// 目標回転量*リクエストデルタタイムでそのフレームでの回転角度を求めることができる
			// リクエストデルタタイムの合算値がちょうど1になるように補正をかけると総回転量は目標回転量と一致する
			bool isFinish = false;
			if (_elapsedTime >= 1)
			{   // 修了確認
				requiredDeltaTime -= (_elapsedTime - 1); // 補正
				isFinish = true;
			}   // 90度進むごとに確認当たってるかどうかを確認する
			else if (_elapsedTime >= 1 / Math.Abs(_polatAngle))
			{
				SetChildHitCheckFloorFlg(true);
				if (_isHitFloor)
				{
					requiredDeltaTime -= (_elapsedTime - 1); // 補正
					isFinish = true;
				}
			}

			// 現在フレームの回転を示す回転のクォータニオン作成
			var angleAxis = Quaternion.AngleAxis(_angle * requiredDeltaTime, _rotAxis);

			// 円運動の位置計算
			var tr = transform;
			var pos = tr.position;
			// クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
			// _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
			pos -= _axisCenterWorldPos;
			pos = angleAxis * pos;
			pos += _axisCenterWorldPos;
			tr.position = pos;

			// 向き更新
			tr.rotation = angleAxis * tr.rotation;

			_oldAngle = _elapsedTime * _angle;

			// ** 終了時処理
			if (!isFinish) return;

			// 終了時に変更するフラグを変更
			_isRotating = false;		// 回転処理の終了
			_isRotateEndFream = true;	// 回転が終わったFを通知

			// 最終的に回転した量
			var finishAngle = _angle;

			// 床に当たってた時の処理
			if (_isHitFloor)
			{
				// 経過時間と補間用数値を用いて現在進んだ角度から一番近い90単位の角度を算出
				finishAngle = (int)Math.Round(_elapsedTime * _polatAngle, 0, MidpointRounding.AwayFromZero) * 90;
				SetReflect(_axisCenterWorldPos, _rotAxis, finishAngle);
			}

			// 最終的に回転した量を考慮して最終補正をクオータニオンで計算する
			// 現在フレームの回転を示す回転のクォータニオン作成
			var qtAngleAxis = Quaternion.AngleAxis(finishAngle, _rotAxis);
			// 円運動の位置計算
			var tmpPos = _oldPos;
			// クォータニオンを用いた回転は原点からのオフセットを用いる必要がある
			// _axisCenterWorldPosを任意軸の座標に変更すれば任意軸の回転ができる
			tmpPos -= _axisCenterWorldPos;
			tmpPos = qtAngleAxis * tmpPos;
			tmpPos += _axisCenterWorldPos;
			// 最新のやつ変更
			this.transform.position = tmpPos;
			// 向き更新
			this.transform.rotation = qtAngleAxis * _oldRotAngle;

			// プレイヤー起因の回転かを判定
			if (_playerTransform != null)
			{
				var playerComp = _playerTransform.GetComponent<Player>();
				// プレイヤーに回転終了通知を飛ばす
				playerComp.NotificationEndRotate();
				// バグ防止
				_playerTransform = null;
			}
			StopPartical();
			Debug.Log("回転終了");
		}
		else
		{

			if(_isRotateEndFream){
				// 普段は当たり判定の処理を切っておく
				SetChildHitCheckFloorFlg(false);
				SetChildHitCheckChainFlg(false);
				// めり込み判定の確認
				SetChildHitCheckInto(true);
			}

			if(_isHitFloor){
				SetReflect(_axisCenterWorldPos, _rotAxis, _angle);
			}

			_doOnce = false;
			_isRotateEndFream = false;
		}
	}
}

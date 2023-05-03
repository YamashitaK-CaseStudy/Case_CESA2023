using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class RotatableObject : MonoBehaviour {

    private Transform _playerTransform = null;// プレイヤーのトランスフォーム

    // 自身の軸でまわす小を開始
    public void StartRotate() {
        if ( _isSpin || _isRotating ) {
            return;
        }

        // フラグを立てる
        _isRotating = true;

        // 経過時間を初期化
        _elapsedTime = 0.0f;

        // トレイルの起動
        PlayPartical();
    }

    public void StartRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle) {

        if ( _isSpin || _isRotating ) {
            return;
        }

        // フラグを立てる
        _isRotating = true;

        // 経過時間を初期化
        _elapsedTime = 0.0f;

        // 回転の中心を設定
        _axisCenterWorldPos = rotCenter;

        // 回転軸を設定
        _rotAxis = rotAxis;

        // 回転オフセット値をセット
        _angle = rotAngle;

        // トレイルの起動
        PlayPartical();
    }

   
    public void StartRotate(Vector3 rotCenter, Vector3 rotAxis,int rotAngle,Transform playerTransform) {
        
        if ( _isSpin || _isRotating ) {
            return;
        }

        // トランスフォームを格納
        _playerTransform = playerTransform;

        // フラグを立てる
        _isRotating = true;

        // 経過時間を初期化
        _elapsedTime = 0.0f;

        // 誤差を修正する
        var pos = new Vector3(0,0,0);
        pos.x = (float)Math.Round(this.transform.position.x, 0, MidpointRounding.AwayFromZero);
        pos.y = (float)Math.Round(this.transform.position.y, 0, MidpointRounding.AwayFromZero);
        pos.z = (float)Math.Round(this.transform.position.z, 0, MidpointRounding.AwayFromZero);
        this.transform.position = pos;

        // 回転の中心を設定
        _axisCenterWorldPos = rotCenter;
        
        // 回転軸を設定
        _rotAxis = rotAxis;

        // 回転オフセット値をセット
        _angle = rotAngle;

        // トレイルの起動
        PlayPartical();
    }

    // まわす小の更新
    protected void UpdateRotate()
    {
        if (_doOnce) {
            _isRotateStartFream = false;
        }

        // 回転中かフラグ
        if ( _isRotating ) {

            if (!_doOnce) {
                _isRotateStartFream = true;
                _doOnce = true;
            }
        
            // リクエストデルタタイムを求める
            // リクエストデルタタイム：デルタタイムを1回転に必要な時間で割った値
            // これの合算値が1になった時,1回転に必要な時間が経過したことになる
            float requiredDeltaTime = Time.deltaTime/_rotRequirdTime;
            _elapsedTime += requiredDeltaTime;

            // 目標回転量*リクエストデルタタイムでそのフレームでの回転角度を求めることができる
            // リクエストデルタタイムの合算値がちょうど1になるように補正をかけると総回転量は目標回転量と一致する
            if ( _elapsedTime >= 1 ) {
                //Debug.Log("補間終了");
                _isRotating = false;
                requiredDeltaTime -= ( _elapsedTime - 1 ); // 補正

                _isRotateEndFream = true;
                StopPartical();

                // プレイヤー起因の回転かを判定
                if ( _playerTransform != null ) {
                    Debug.Log(_playerTransform.gameObject);
                
                    var playerComp = _playerTransform.GetComponent<Player>();

                    if ( playerComp == null ) {
                        Debug.Log("ありえない話");
                    }

                    // プレイヤーに回転終了通知を飛ばす
                    playerComp.NotificationEndRotate();

                    // バグ防止
                    _playerTransform = null;
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
        }
        else {
            _doOnce = false;
            _isRotateEndFream = false;
        }
    }


}

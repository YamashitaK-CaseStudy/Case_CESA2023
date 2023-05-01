using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class RotatableObject : MonoBehaviour
{

    private Transform _playerTransform = null;// プレイヤーのトランスフォーム

    public bool _isReservation = false;// 予約フラグ
    public Vector3 _resePos { get; set; }   // 予約座標
    public Vector3 _reseAxis { get; set; }  // 予約軸
    public int _reseAngle { get; set; }     // 予約回転
    private float _oldAngle = 0.0f;
    private RotHitFloar[] _childComp;
    private void StartSettingRot()
    {
        var child = this.transform.GetChild(0).gameObject;
        _childComp = new RotHitFloar[child.transform.childCount];
        for (int i = 0; i < child.transform.childCount; i++)
        {
            _childComp[i] = child.transform.GetChild(i).GetComponent<RotHitFloar>();
        }
    }

    public void StartRotate(Vector3 rotCenter, Vector3 rotAxis, int rotAngle)
    {

        if (_isSpin || _isRotating)
        {
            return;
        }
        // 回転の中心を設定
        _axisCenterWorldPos = rotCenter;
        _resePos = rotCenter;

        // 回転軸を設定
        _rotAxis = rotAxis;
        _reseAxis = rotAxis;

        // 回転オフセット値をセット
        _angle = rotAngle;
        _reseAngle = rotAngle;
        for (int i = 0; i < _childComp.Length; i++)
        {
            _childComp[i].ChecktoRotate(_resePos, _reseAxis, _reseAngle);
        }
        // フラグを立てる
        _isRotating = true;

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

        // 回転の中心を設定
        _axisCenterWorldPos = rotCenter;
        _resePos = rotCenter;

        // 回転軸を設定
        _rotAxis = rotAxis;
        _reseAxis = rotAxis;

        // 回転オフセット値をセット
        _angle = rotAngle;
        _reseAngle = rotAngle;

        for (int i = 0; i < _childComp.Length; i++)
        {
            _childComp[i].ChecktoRotate(_resePos, _reseAxis, _reseAngle);
        }

        // トランスフォームを格納
        _playerTransform = playerTransform;

        // フラグを立てる
        _isRotating = true;

        // 経過時間を初期化
        _elapsedTime = 0.0f;

        // トレイルの起動
        PlayPartical();
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
            float requiredDeltaTime = Time.deltaTime / _rotRequirdTime * _angle;
			Debug.Log(">>>>>");
			Debug.Log(_angle);
            _elapsedTime += requiredDeltaTime;

            // 目標回転量*リクエストデルタタイムでそのフレームでの回転角度を求めることができる
            // リクエストデルタタイムの合算値がちょうど1になるように補正をかけると総回転量は目標回転量と一致する
            if (_elapsedTime >= 1)
            {
                _isRotating = false;
                requiredDeltaTime -= (_elapsedTime - 1); // 補正

                _isRotateEndFream = true;
                StopPartical();

                // プレイヤー起因の回転かを判定
                if (_playerTransform != null)
                {
                    var playerComp = _playerTransform.GetComponent<Player>();

                    // プレイヤーに回転終了通知を飛ばす
                    playerComp.NotificationEndRotate();

                    // バグ防止
                    _playerTransform = null;
                }
                for (int i = 0; i < _childComp.Length; i++)
                {
                    _childComp[i].InitCheckCollider();
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
        }
        else
        {
            _doOnce = false;
            _isRotateEndFream = false;
        }
    }

    // 回転を強制終了
    public void ForcedStopRotate()
    {
        _isRotating = false;
        _elapsedTime = 0.0f;

        _isRotateEndFream = true;
        Debug.Log(_resePos);
        Debug.Log(-_reseAxis);
        Debug.Log(_oldAngle);
        StartRotate(_resePos, -_reseAxis, Math.Abs((int)_oldAngle));
    }
}

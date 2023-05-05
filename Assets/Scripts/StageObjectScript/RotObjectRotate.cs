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
    private float _polatAngle = 0.0f;
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
        // フラグを立てる
        _isRotating = true;

        // 角度による補正値を計算する
        _polatAngle = _angle / 90;
        Debug.Log(_polatAngle);

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

        // トランスフォームを格納
        _playerTransform = playerTransform;

        // フラグを立てる
        _isRotating = true;

        // 経過時間を初期化
        _elapsedTime = 0.0f;

        // 角度による補正値を計算する
        _polatAngle = _angle / 90;
        Debug.Log(_polatAngle);

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
            float requiredDeltaTime = Time.deltaTime / (_rotRequirdTime * Math.Abs(_polatAngle));
            _elapsedTime += requiredDeltaTime;

            // 目標回転量*リクエストデルタタイムでそのフレームでの回転角度を求めることができる
            // リクエストデルタタイムの合算値がちょうど1になるように補正をかけると総回転量は目標回転量と一致する
            bool isFinish = false;
            if (_elapsedTime >= 1)
            {
                _isRotating = false;
                _isRotateEndFream = true;
                requiredDeltaTime -= (_elapsedTime - 1); // 補正

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
                isFinish = true;
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
            // 90度進むごとに確認当たってるかどうかを確認する
            if(Math.Abs(_polatAngle) > 1){    // 90度が1になるのでそれ以上かどうか確認
                if(_elapsedTime >= 1 / Math.Abs(_polatAngle)){
                    Debug.Log(_elapsedTime);
                    Debug.Log("90度経過");
                    isFinish = true;
                    _isRotating = false;

                    // 緊急停止しているので角度に補正を書けないと誤差が出る
                    var tmpAngle = this.transform.eulerAngles;
                    // if(_rotAxis.x != 0){
                    //     tmpAngle.x = _polatAngle * 90;
                    // }else if(_rotAxis.y != 0){
                    //     tmpAngle.y = _polatAngle * 90;
                    // }
                    this.transform.eulerAngles = tmpAngle;
                    _polatAngle = 0.0f;
                }
            }

            if(isFinish){
                // 誤差を修正する
                var tmppos = new Vector3(0, 0, 0);
                tmppos.x = (float)Math.Round(this.transform.position.x, 0, MidpointRounding.AwayFromZero);
                tmppos.y = (float)Math.Round(this.transform.position.y, 0, MidpointRounding.AwayFromZero);
                tmppos.z = (float)Math.Round(this.transform.position.z, 0, MidpointRounding.AwayFromZero);
                this.transform.position = tmppos;

                _isRotating = false;
                _elapsedTime = 0.0f;
                _isRotateEndFream = true;

                CheckHitNotMoveObj();
            }
        }
        else
        {
            _doOnce = false;
            _isRotateEndFream = false;
        }
    }

    private void CheckHitNotMoveObj()
    {
        if (!_isReservation) return;
        _isReservation = false;
        StartRotate(_resePos, -_reseAxis, _reseAngle);
    }
}

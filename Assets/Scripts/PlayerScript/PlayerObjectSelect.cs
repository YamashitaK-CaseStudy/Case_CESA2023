using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour
{
    [SerializeField] GameObject _pf_SelectRange;
    [SerializeField] float _RangeMagnification = 1.0f;
    public GameObject _selectGameObject { get; set; } = null;
    private SphereCollider _collider;
    private float _SelectRangeRadius;
    private LineRenderer _linerendere;

    [Obsolete]
    void StartObjectSelect() {

        // スケール設定
        _pf_SelectRange.transform.localScale = new Vector3(0, 1, 1) * _RangeMagnification;

        // 範囲の半径を取得
        _collider = _pf_SelectRange.gameObject.GetComponent<SphereCollider>();
        _SelectRangeRadius = _collider.radius * Mathf.Max(_pf_SelectRange.transform.lossyScale.x, _pf_SelectRange.transform.lossyScale.y, _pf_SelectRange.transform.lossyScale.z);

        _linerendere = _pf_SelectRange.GetComponent<LineRenderer>();
        _linerendere.SetWidth(0.1f, 0.1f);
    }

    void UpdateObjectSelect() {

        var pos = this.gameObject.transform.position;

        // 範囲オブジェクトをプレイヤーの座標に入れ続ける
        _pf_SelectRange.gameObject.transform.position = new Vector3(pos.x, pos.y, 1);

        // 円の中に入ってる選択できるオブジェクトリスト
        var inobjects = GetSelectRangeObjects(_SelectRangeRadius);

        // 円の中に入っているオブジェクトから一番プレイヤーから近いオブジェクトを取得する
        var nearobject = GetInObjectNearObject(ref inobjects);

        _linerendere.SetPosition(0, pos);
       
        if (nearobject != null) {
            _linerendere.SetPosition(1, nearobject.transform.position);
            _selectGameObject = nearobject;
            Debug.Log(nearobject.name);
        }
        else {
            _linerendere.SetPosition(1, pos);
            _selectGameObject = null;
            Debug.Log(null);
        }
    }

    // 円の中に入ってるオブジェクトを取得
    private List<GameObject> GetSelectRangeObjects(float _selectrangeradius) {

        // ワールド内の回転できるオブジェクトを集める
        GameObject[] rotateobjects = GameObject.FindGameObjectsWithTag("RotateObject");

        // 円の中に入ってるオブジェクトを集める
        List<GameObject> in_Objects = new List<GameObject>();
        foreach (var _object in rotateobjects) {

            // プレイヤーとの距離を計算
            var distance = Vector3.Distance(this.gameObject.transform.position, _object.transform.position);

            if (distance <= _SelectRangeRadius) {
                in_Objects.Add(_object);
            }
        }

        return in_Objects;
    }

    private GameObject GetInObjectNearObject(ref List<GameObject> list) {

        GameObject nearObject = null;
        float neardistance = 0.0f;

        // プレイヤーから一番近いオブジェクトを取得
        for (int i = 0; i < list.Count; i++) {

            // プレイヤーとの距離を計算
            float distance = Vector3.Distance(this.gameObject.transform.position, list[i].transform.position);

            if (i == 0) {
                nearObject = list[i];
                neardistance = Math.Abs(distance);
            }
            else {
                if (Math.Abs(distance) < neardistance) {
                    nearObject = list[i];
                    neardistance = Math.Abs(distance);
                }
            }
        }

        return nearObject;
    }

}

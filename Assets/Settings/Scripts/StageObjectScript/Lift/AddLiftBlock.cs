using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddLiftBlock : MonoBehaviour
{
    [SerializeField] private int _addRightBlockNum; 
    [SerializeField] private int _addLeftBlockNum; 
    [SerializeField] private int _addUpBlockNum; 
    [SerializeField] private int _addDownBlockNum;

    // リフトコンポーネント
    private Lift _lift;
   
    public void ApplyInspector() {

        // 親からリフトを取得する
        _lift = InitGetLiftComponent();

        var pos = this.transform.localPosition;

        // 右
        for (int i = 0; i < _addRightBlockNum; i++) {

            // 生成したいローカル位置
            var calPos = new Vector3(pos.x + i + 1, pos.y, 0);

            // 生成可能なのか判断
            RideObjPosSameToSet(calPos, _lift);
        }

        // 左
        for (int i = 0; i < _addLeftBlockNum; i++) {

            // 生成したいローカル位置
            var calPos = new Vector3(pos.x - i - 1, pos.y, 0);

            // 生成可能なのか判断
            RideObjPosSameToSet(calPos, _lift);
        }

        // 上
        for (int i = 0; i < _addUpBlockNum; i++) {

            // 生成したいローカル位置
            var calPos = new Vector3(pos.x, pos.y + i + 1, 0);

            // 生成可能なのか判断
            RideObjPosSameToSet(calPos, _lift);
        }

        // 下
        for (int i = 0; i < _addDownBlockNum; i++) {

            // 生成したいローカル位置
            var calPos = new Vector3(pos.x, pos.y - i - 1, 0);

            // 生成可能なのか判断
            RideObjPosSameToSet(calPos, _lift);
        }
    }

    private Lift InitGetLiftComponent() {

         _lift = this.transform.root.GetComponent<Lift>();

        if(_lift == null) {
            return _lift;
            Debug.Log("リフトのインスタンスがありません");
        }
        else {
            return _lift;
        }
    }



    public void BlockDesroy() {

        DestroyImmediate(this.gameObject);
    }




    private void RideObjPosSameToSet(in Vector3 fuePos, Lift lift) {

        bool isCreate = true;

        for(int i = 0;i < lift.GetRideBlocks.transform.childCount; i++) {
            var pos = lift.GetRideBlocks.transform.GetChild(i).localPosition;
            if(fuePos == pos) {
                isCreate = false;
                return;
            }
        }

        if (isCreate) {
            var r_rideObj = Instantiate(lift.GetCopyRideBlock, lift.GetRideBlocks.transform);
            r_rideObj.transform.localPosition = fuePos;
        }
    }

}

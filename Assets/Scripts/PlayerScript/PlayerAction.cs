using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{


    [SerializeField] private float _powerRJ;
    [SerializeField] private int   _needFrameRJ;

    private bool _isRJ = false;
    private int _elapsedFrameRJ = 0;
 
    private Vector3[] _locusPosRJ;

    enum INPUT_DIRECTION {
        NONE = 0,
        UP,
        DOWN,
        LEFT,
        RIGHT,
        R_UP,
        R_DOWN,
        L_UP,
        L_DOWN
    }



    // Start is called before the first frame update
    void StartAction()
    {
        
    }

    // Update is called once per frame
    void UpdateAction(){

        if ( _isRJ ) {
            transform.position = _locusPosRJ[_elapsedFrameRJ];
            _elapsedFrameRJ++;
        }

        if ( _elapsedFrameRJ >= _needFrameRJ ) {
            _isRJ = false;
            _locusPosRJ = null;
        }
    }

    void StartRollingJumpGround(INPUT_DIRECTION inputDir) {
        Vector3 startPos = this.transform.position;
        float secPerFrame = 60/_needFrameRJ;

        Vector3 forceRJ = Vector3.zero;

        switch ( inputDir ) {
        case INPUT_DIRECTION.LEFT:
            forceRJ.x = _powerRJ * Mathf.Cos(150 * Mathf.Deg2Rad);
            forceRJ.y = _powerRJ * Mathf.Sin(150 * Mathf.Deg2Rad);
            break;
       
        case INPUT_DIRECTION.RIGHT:
            forceRJ.x = _powerRJ * Mathf.Cos(30 * Mathf.Deg2Rad);
            forceRJ.y = _powerRJ * Mathf.Sin(30 * Mathf.Deg2Rad);
            break;

        default:
            Debug.Log("呼び出す引数が違います");
            break;
        }

        // フレーム単位でのプレイヤーの座標を計算する
        for ( int i = 0 ; i < _needFrameRJ ; i++ ) {
            Vector3 movementAmount = new Vector3(0.0f,0.0f,0.0f);
            float t = secPerFrame * i;

            movementAmount.x = forceRJ.x * t;
            movementAmount.y = (float)( ( forceRJ.y * t ) - ( 0.5 * 9.81 * t * t ) );
            movementAmount.z = 0.0f;

            _locusPosRJ[i] = movementAmount + startPos;
        }

        // 必要変数の初期化
        _elapsedFrameRJ = 0;
        _isRJ = true;

    }

    void StartRollingJumpSky(INPUT_DIRECTION inputDir) {

        Vector3 startPos = this.transform.position;
        float secPerFrame = 60/_needFrameRJ;

        // ジャンプ力を入力方向に応じてベクトルに分解
        Vector3 forceRJ = Vector3.zero;
        switch ( inputDir ) {

        case INPUT_DIRECTION.UP:
            forceRJ.y = _powerRJ;
            break;

        case INPUT_DIRECTION.DOWN:
            forceRJ.y = -_powerRJ;
            break;

        case INPUT_DIRECTION.LEFT:
            forceRJ.x = -_powerRJ;
            break;

        case INPUT_DIRECTION.RIGHT:
            forceRJ.x = _powerRJ;
            break;

        case INPUT_DIRECTION.R_UP:
            forceRJ.x = _powerRJ * Mathf.Cos(45 * Mathf.Deg2Rad);
            forceRJ.y = _powerRJ * Mathf.Sin(45 * Mathf.Deg2Rad);
            break;

        case INPUT_DIRECTION.R_DOWN:
            forceRJ.x = _powerRJ * Mathf.Cos(315 * Mathf.Deg2Rad);
            forceRJ.y = _powerRJ * Mathf.Sin(315 * Mathf.Deg2Rad);
            break;

        case INPUT_DIRECTION.L_UP:
            forceRJ.x = _powerRJ * Mathf.Cos(135 * Mathf.Deg2Rad);
            forceRJ.y = _powerRJ * Mathf.Sin(135 * Mathf.Deg2Rad);
            break;

        case INPUT_DIRECTION.L_DOWN:
            forceRJ.x = _powerRJ * Mathf.Cos(225 * Mathf.Deg2Rad);
            forceRJ.y = _powerRJ * Mathf.Sin(225 * Mathf.Deg2Rad);
            break;


        default:
            break;
        }

        // フレーム単位でのプレイヤーの座標を計算する
        for ( int i = 0 ; i < _needFrameRJ ; i++ ) {
            Vector3 movementAmount = new Vector3(0.0f,0.0f,0.0f);
            float t = secPerFrame * i;

            movementAmount.x = forceRJ.x * t;
            movementAmount.y = (float)( ( forceRJ.y * t ) - ( 0.5 * 9.81 * t * t ) );
            movementAmount.z = 0.0f;

            _locusPosRJ[i] = movementAmount + startPos;
        }

        // 必要変数の初期化
        _elapsedFrameRJ = 0;
        _isRJ = true;


    }


}

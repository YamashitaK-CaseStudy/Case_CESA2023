using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ResultMenu : MonoBehaviour
{
    [SerializeField] List<Sprite> _image;
    int _dispImageNum = 0;
    int _inputAxis = 0;
    bool _isInput = false;
    bool _isCanInput = false;
    float _reqTime = 0;
    float _reqTimeInput = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = _image[_dispImageNum];
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isCanInput && this.gameObject.activeSelf == true){
            _reqTimeInput += Time.deltaTime;
            if(_reqTimeInput > 0.5){
                _isCanInput = true;
                _reqTimeInput = 0.0f;
            }
        }
        _reqTime += Time.deltaTime;
        if(_reqTime <= 0.15)return;
        _reqTime = 0f;
        Input();
    }

    void Input(){
        if(!_isCanInput) return;
        if(_isInput){
            _dispImageNum -= _inputAxis;
            if(_dispImageNum == 3) _dispImageNum = 0;
            if(_dispImageNum == -1) _dispImageNum = 2;
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = _image[_dispImageNum];
            SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Select);
        }
    }

    public void OnDecision(InputAction.CallbackContext context){
        if(!_isCanInput) return;
        if(!context.performed) return;

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Decision1);
        SystemSoundManager.Instance.StopBGMWithFade(1.5f);
        SuzumuraTomoki.SceneManager.playerInput.Disable();
        switch (_dispImageNum){
            case 0: // NextStage
            SuzumuraTomoki.SceneManager.LoadNextScene();
            break;
            case 1: // Retry
            SuzumuraTomoki.SceneManager.LoadBeforeScene();
            break;
            case 2: // StageSelect
            SuzumuraTomoki.SceneManager.LoadStageSelect();
            break;
        }
    }
    public void OnArrow(InputAction.CallbackContext context){
        if(!_isCanInput) return;
        if(context.started){
            _inputAxis = (int)context.ReadValue<Vector2>().y;
            _isInput = true;
        }
        if(context.canceled){
            _inputAxis = 0;
            _isInput = false;
        }
    }

}

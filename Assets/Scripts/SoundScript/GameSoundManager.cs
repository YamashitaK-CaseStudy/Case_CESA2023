using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class GameSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _gameBgmAudioSource;
    [SerializeField] AudioSource _gameSeAudioSource;

    [SerializeField] List<GameBGMSoundData> _gameBgmSoundDatas;
    [SerializeField] List<GameSESoundData> _gameSeSoundDatas;

    public float _masterVolume = 1;
    public float _seMasterVolume = 1;
    public float _bgmMasterVolume = 1;

    public static GameSoundManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayGameBGM(GameBGMSoundData.GameBGM bgm) {
        GameBGMSoundData data = _gameBgmSoundDatas.Find(data => data._gameBgm == bgm);
        _gameSeAudioSource.volume = data._gameBgmVolume * _bgmMasterVolume * _masterVolume;
        _gameBgmAudioSource.loop = true;
        _gameBgmAudioSource.clip = data._gameBgmAudioClip;
        _gameBgmAudioSource.Play();
       
    }

    public void PlayGameBGMWithFade(GameBGMSoundData.GameBGM bgm , float vol , float duration) {
        GameBGMSoundData data = _gameBgmSoundDatas.Find(data => data._gameBgm == bgm);
        _gameBgmAudioSource.loop = true;
        _gameBgmAudioSource.clip = data._gameBgmAudioClip;
        _gameBgmAudioSource.volume = 0;
        _gameBgmAudioSource.Play();
        _gameBgmAudioSource.DOFade(vol, duration);
    }

    public void StopGameBGMWithFade(float duration) {
        _gameBgmAudioSource.DOFade(0, duration);
    }

    public void PlayGameSE(GameSESoundData.GameSE se)
    {
        GameSESoundData data = _gameSeSoundDatas.Find(data => data._gameSe == se);
        _gameSeAudioSource.volume = data._gameSeVolume * _seMasterVolume * _masterVolume;
        _gameSeAudioSource.PlayOneShot(data._gameSeAudioClip);
    }
    public void PlayGameSEWithFade(GameSESoundData.GameSE se , float vol , float duration) {
        GameSESoundData data = _gameSeSoundDatas.Find(data => data._gameSe == se);
        _gameBgmAudioSource.loop = true;
        _gameBgmAudioSource.clip = data._gameSeAudioClip;
        _gameBgmAudioSource.volume = 0;
        _gameBgmAudioSource.Play();
        _gameBgmAudioSource.DOFade(vol, duration);
    }

    public void StopGameSEWithFade(float duration) {
        _gameBgmAudioSource.DOFade(0, duration);
    }
}


[System.Serializable]
public class GameBGMSoundData {

    // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
    public enum GameBGM {
        Roborder,
    }

    public GameBGM _gameBgm;
    public AudioClip _gameBgmAudioClip;
    [Range(0, 1)]
    public float _gameBgmVolume = 1;
}

[System.Serializable]
public class GameSESoundData
{
    // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
    public enum GameSE
    {
        Rotate,
        Spin,
        Magnet,
        Chain,
        Spark,
        Bomb,
        Conveyor,
        Shutter,
        Reflect,
        Projection,
        Kidou,
        Lift,
        GetKey,
        OpenDoor
     }

    public GameSE _gameSe;
    public AudioClip _gameSeAudioClip;
    [Range(0, 1)]
    public float _gameSeVolume = 1;
}
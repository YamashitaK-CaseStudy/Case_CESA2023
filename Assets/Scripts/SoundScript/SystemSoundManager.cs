using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;


public class SystemSoundManager : MonoBehaviour {
    [SerializeField] AudioSource _bgmAudioSource;
    [SerializeField] AudioSource _seAudioSource;

    [SerializeField] List<BGMSoundData> _bgmSoundDatas;
    [SerializeField] List<SystemSESoundData> _seSoundDatas;

    public float _masterVolume = 1;
    public float _bgmMasterVolume = 1;
    public float _seMasterVolume = 1;

    public static SystemSoundManager Instance {
        get; private set;
    }

    private void Awake() {
        if ( Instance == null ) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(BGMSoundData.BGM bgm) {
        BGMSoundData data = _bgmSoundDatas.Find(data => data._bgm == bgm);
        _bgmAudioSource.clip = data._audioClip;
        _bgmAudioSource.volume = data._volume * _bgmMasterVolume * _masterVolume;
        _bgmAudioSource.Play();
    }

    public void PlayBGMWithFade(BGMSoundData.BGM bgm, float vol, float duration) {
        BGMSoundData data = _bgmSoundDatas.Find(data => data._bgm == bgm);
        _bgmAudioSource.loop = true;
        _bgmAudioSource.clip = data._audioClip;
        _bgmAudioSource.volume = 0;
        _bgmAudioSource.Play();
        _bgmAudioSource.DOFade(vol, duration);
    }

    public void StopBGMWithFade(float duration) {
        _bgmAudioSource.DOFade(0, duration);
    }

    public void BGMFade(float vol,float duration)
    {
        _bgmAudioSource.DOFade(vol, duration);
    }

    public void PlaySE(SystemSESoundData.SystemSE se) {
        SystemSESoundData data = _seSoundDatas.Find(data => data._systemSe == se);
        _seAudioSource.volume = data._systemSeVolume * _seMasterVolume * _masterVolume;
        _seAudioSource.PlayOneShot(data._systemSeAudioClip);
    }
}

[System.Serializable]
public class BGMSoundData {
         
    // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
    public enum BGM {
        Title,
        StageSelect,
        MainGame,
        Result,
    }

    public BGM _bgm;
    public AudioClip _audioClip;
    [Range(0, 1)]
    public float _volume = 1;
}

[System.Serializable]
public class SystemSESoundData {
    // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
    public enum SystemSE {
        Enter,
        Cancel,
        Select,
        Error,
        Decision1,
        Decision2,
        Decision3,
        Failed,
        ToSelect,
        Slide,
    }

    public SystemSE _systemSe;
    public AudioClip _systemSeAudioClip;
    [Range(0, 1)]
    public float _systemSeVolume = 1;
}
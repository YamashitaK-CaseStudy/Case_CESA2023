using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _gameSeAudioSource;

    [SerializeField] List<GameSESoundData> _gameSeSoundDatas;

    public float _masterVolume = 1;
    public float seMasterVolume = 1;

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

    public void PlayGameSE(GameSESoundData.GameSE se)
    {
        GameSESoundData data = _gameSeSoundDatas.Find(data => data._gameSe == se);
        _gameSeAudioSource.volume = data._gameSeVolume * seMasterVolume * _masterVolume;
        _gameSeAudioSource.PlayOneShot(data._gameSeAudioClip);
    }
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
     }

    public GameSE _gameSe;
    public AudioClip _gameSeAudioClip;
    [Range(0, 1)]
    public float _gameSeVolume = 1;
}
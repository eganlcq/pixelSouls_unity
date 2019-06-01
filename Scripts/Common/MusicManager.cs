using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PATH_PREFAB = "Prefab/Scene Management/MusicManager";
    private static MusicManager _instance;
    public static MusicManager Instance {

        get {

            if (_instance == null) {

                GameObject obj = Instantiate(Resources.Load(PATH_PREFAB)) as GameObject;
                _instance = obj.GetComponent<MusicManager>();
            }

            return _instance;
        }
    }

    private AudioSource[] _player;
    private IEnumerator[] _fader = new IEnumerator[2];
    private int _activePlayer = 0;
    private readonly int _volumeChangePerSecond = 15;

    [Header("-- FADE --")]
    [SerializeField]
    private readonly float fadeDuration = 2f;

    [Header("-- VOLUME --")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _volume = 0.2f;

    [Header("-- SOUNDS --")]
    [SerializeField]
    private List<AudioClip> _clips = new List<AudioClip>();
    [SerializeField]
    private List<Music> _matchingMusic = new List<Music>();
    private Dictionary<Music, AudioClip> _audioData = new Dictionary<Music, AudioClip>();

    public bool Mute {

        get { return _player[_activePlayer].mute; }
        set { foreach (AudioSource source in _player) source.mute = value; }
    }

    private void Awake() {

        DontDestroyOnLoad(gameObject);
        _player = new AudioSource[2] {

            gameObject.AddComponent<AudioSource>(),
            gameObject.AddComponent<AudioSource>()
        };

        foreach(AudioSource source in _player) {

            source.loop = true;
            source.playOnAwake = false;
            source.volume = 0f;
        }

        for(int i = 0; i < _clips.Count; i++) {

            _audioData.Add(_matchingMusic[i], _clips[i]);
        }
    }

    public void Play(Music music) {

        if (_audioData[music] == _player[_activePlayer].clip && _player[_activePlayer].volume > 0) return;

        foreach(IEnumerator coroutine in _fader) {

            if (coroutine != null) StopCoroutine(coroutine);
        }

        if(_player[_activePlayer].volume > 0) {

            _fader[0] = FadeAudioSource(_player[_activePlayer], fadeDuration, 0f, () => { _fader[0] = null; });
            StartCoroutine(_fader[0]);
        }

        int nextPlayer = (_activePlayer + 1) % _player.Length;
        _player[nextPlayer].clip = _audioData[music];
        _player[nextPlayer].Play();
        _fader[1] = FadeAudioSource(_player[nextPlayer], fadeDuration, _volume, () => { _fader[1] = null; });
        StartCoroutine(_fader[1]);

        _activePlayer = nextPlayer;
    }

    public void StopMusic() {

        foreach (IEnumerator coroutine in _fader) {

            if (coroutine != null) StopCoroutine(coroutine);
        }

        if (_player[_activePlayer].volume > 0) {

            _player[_activePlayer].volume = 0f;
        }
    }

    public void PlayNoFade(Music music) {

        if (_audioData[music] == _player[_activePlayer].clip) return;

        foreach (IEnumerator coroutine in _fader) {

            if (coroutine != null) StopCoroutine(coroutine);
        }

        if (_player[_activePlayer].volume > 0) {

            _player[_activePlayer].volume = 0f;
        }

        int nextPlayer = (_activePlayer + 1) % _player.Length;
        _player[nextPlayer].clip = _audioData[music];
        _player[nextPlayer].volume = _volume;
        _player[nextPlayer].Play();

        _activePlayer = nextPlayer;
    }

    private IEnumerator FadeAudioSource(AudioSource player, float duration, float targetVolume, Action finishedCallback = null) {

        int steps = (int)(_volumeChangePerSecond * duration);
        float stepTime = duration / steps;
        float stepSize = (targetVolume - player.volume) / steps;

        for(int i = 1; i < steps; i++) {

            player.volume += stepSize;
            yield return new WaitForSeconds(stepTime);
        }

        player.volume = targetVolume;

        if (finishedCallback != null) finishedCallback();
    }

    public enum Music
    {
        Menu,
        ChooseCharacter,
        Fight,
        EndFight
    }
}

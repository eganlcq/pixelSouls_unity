using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private AudioSource _source;

    private void Awake() {

        _source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip) {

        _source.PlayOneShot(clip);
    }
}

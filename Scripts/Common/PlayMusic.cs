using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField]
    private MusicManager.Music _musicToPlay;
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Instance.Play(_musicToPlay);
    }
}

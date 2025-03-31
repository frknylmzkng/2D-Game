using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource introSource, loopSource;

    void Start()
    {
        //1. m�zik bitti�inde 2. m�zik �alacak ve 2. m�zik loop halinde kalacak
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }
}

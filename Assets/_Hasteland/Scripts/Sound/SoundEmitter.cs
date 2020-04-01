using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    AudioSource m_aSource;

    void Start()
    {
        m_aSource = GetComponent<AudioSource>();
        
    }

    public void PlayClip()
    {
        print(gameObject.name + " Fire Sound");
        m_aSource.Stop();
        m_aSource.Play();
    }
}

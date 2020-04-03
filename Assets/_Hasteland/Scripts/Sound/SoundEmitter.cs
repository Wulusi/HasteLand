using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    AudioSource m_aSource;
    public List<AudioClip> m_allClips;

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
    public void PlayRandomClip()
    {
        m_aSource.Stop();
        m_aSource.clip = m_allClips[Random.Range(0, m_allClips.Count)];
        m_aSource.Play();
    }
}

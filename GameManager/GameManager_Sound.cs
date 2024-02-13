using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Sound : MonoBehaviour
{
    [Header("Music List")]
    [SerializeField] List<AudioClip> _audioClips_Ambient = new List<AudioClip>();
    [Header("Audio Source")]
    [SerializeField] AudioSource m_AudioSource;
    void Start()
    {
        m_AudioSource.clip = _audioClips_Ambient[Random.Range(0, _audioClips_Ambient.Count)];
        m_AudioSource.Play();
    }

}

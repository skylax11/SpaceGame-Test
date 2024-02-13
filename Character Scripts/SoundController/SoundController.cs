using Assets.Scripts.Character_Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonobehaviourSingleton<SoundController>
{
    [Header("Walk Sounds List")]
    [SerializeField] List<AudioClip> _audioClips_Walk = new List<AudioClip>();

    [Header("Audio Sources")]
    [SerializeField] public AudioSource m_AudioSource_Walk;
    [SerializeField] AudioSource m_AudioSource_Weapon;
    public void PlayWalkSound(float volume,float pitch)
    {
        if (!m_AudioSource_Walk.isPlaying && PlayerInputManager.Instance.direction != Vector2.zero)
        {
            m_AudioSource_Walk.volume = volume;
            m_AudioSource_Walk.pitch = pitch;
            m_AudioSource_Walk.clip = _audioClips_Walk[Random.Range(0, _audioClips_Walk.Count)];
            m_AudioSource_Walk.Play();
        }
    }
    public void PlayWeaponSound(float volume, float pitch,AudioClip clip)
    {
        if ((m_AudioSource_Weapon.time < m_AudioSource_Weapon.clip.length / 1.2))
        {
            m_AudioSource_Weapon.volume = volume;
            m_AudioSource_Weapon.pitch = pitch;
            m_AudioSource_Weapon.clip = clip;
            m_AudioSource_Weapon.Play();
        }
    }
}

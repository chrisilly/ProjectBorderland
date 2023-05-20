using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;

    public AudioClip phaseShiftSFX;
    public AudioClip walkingSFX;
    public AudioClip dashSFX;
    public AudioClip deathSFX;
    public AudioClip jumpSFX;
    public AudioClip climbingSFX;
    public AudioClip checkpointSFX;
    public AudioClip clickSFX;

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

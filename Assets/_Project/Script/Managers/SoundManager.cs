using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public bool soundOn;
    public GameObject soundCheck;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        soundOn = true;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void ToggleVolume()
    {
        soundOn = !soundOn;
        soundCheck.SetActive(soundOn);
    }

    // SoundManager.Instance.PlayRandomPitch(audioClip, (volume);
    public void PlayRandomPitch(AudioClip clip, float volume = 1f)
    {
        if (!soundOn)
            return;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip, volume);
        audioSource.pitch = 1f;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (!soundOn)
            return;
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayRandomSound(List<AudioClip> clips, float volume = 1f)
    {
        if (!soundOn || clips.Count == 0)
            return;
        int randomIndex = Random.Range(0, clips.Count);
        PlayRandomPitch(clips[randomIndex], volume);
    }
}

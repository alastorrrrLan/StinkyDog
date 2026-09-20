using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public static UIAudio Instance;

    public AudioSource source;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    void Awake()
    {
        Instance = this;
        if (source == null)
            source = GetComponent<AudioSource>();
    }

    public void PlayHover()
    {
        if (hoverClip != null)
            source.PlayOneShot(hoverClip, 0.6f);
    }

    public void PlayClick()
    {
        if (clickClip != null)
            source.PlayOneShot(clickClip, 0.8f);
    }
}
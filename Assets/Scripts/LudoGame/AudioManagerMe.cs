using UnityEngine;
using UnityEngine.UI;

public class AudioManagerMe : MonoBehaviour
{
    public static AudioManagerMe Instance;
    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Ludo SFX")]
    public AudioClip diceRoll;
    public AudioClip diceResult;
    public AudioClip pawnMove;
    public AudioClip pawnEnter;
    public AudioClip capture;
    public AudioClip error;
    public AudioClip win;
    public AudioClip rankingOpen;

    public AudioClip buttonClick;

    [Header("Volume")]
    public Slider volumeSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (sfxSource != null)
            sfxSource.volume = volume;

        if (volumeSlider != null)
            volumeSlider.value = volume;
    }
}
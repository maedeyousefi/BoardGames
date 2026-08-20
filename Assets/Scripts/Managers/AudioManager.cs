using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Music")]
    public AudioSource musicSource;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("SFX")]
    public AudioClip diceRoll;
    public AudioClip pawnMove;
    public AudioClip snake;
    public AudioClip ladder;
    public AudioClip win;
    public Slider volumeSlider;
    public AudioClip buttonClick;

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
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
    public void SetSFXVolume(float volume)
    {
        Debug.Log("Volume = " + volume);

        if (sfxSource != null)
            sfxSource.volume = volume;

        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        sfxSource.volume = volume;

        if (volumeSlider != null)
            volumeSlider.value = volume;
    }
}
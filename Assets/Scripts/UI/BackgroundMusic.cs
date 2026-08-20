using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }


    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);

        audioSource.volume = volume;
    }
}
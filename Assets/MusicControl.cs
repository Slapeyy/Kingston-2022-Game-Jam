using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public static MusicControl Instance { get; private set; }

    public AudioSource source;

    public AudioClip happyLoop, ominousLoop, menuLoop, gameOver;

    public void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        source.volume = PlayerPrefs.GetFloat("Music");
    }

    public void PlayHappyLoop()
    {
        source.loop = true;
        source.clip = happyLoop;
        source.Play();
        source.volume = PlayerPrefs.GetFloat("Music");
    }

    public void PlayOminuousLoop()
    {
        source.loop = true;
        source.clip = ominousLoop;
        source.Play();
        source.volume = PlayerPrefs.GetFloat("Music");
    }

    public void PlayMenuLoop()
    {
        source.loop = true;
        source.clip = ominousLoop;
        source.Play();
        source.volume = PlayerPrefs.GetFloat("Music");
    }

    public void GameOverLoop()
    {
        source.loop = true;
        source.clip = gameOver;
        source.Play();
        source.volume = PlayerPrefs.GetFloat("Music");
    }

    public void StopMusic()
    {
        source.Stop();
        source.loop = false;
        source.clip = null;
    }
}

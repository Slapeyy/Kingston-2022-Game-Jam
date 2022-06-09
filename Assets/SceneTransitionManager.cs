using UnityEngine;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    public AudioSource source;
    public AudioClip glitchSound;

    private bool transition;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Transition(int scene)
    {
        if (!transition)
        {
            StartCoroutine(GlitchApp(scene));
            transition = true;
        }
    }

    public IEnumerator GlitchApp(int scene)
    {
        source.PlayOneShot(glitchSound, PlayerPrefs.GetFloat("Sound") * 0.5f);
        StartCoroutine(GlitchAnim());
        yield return new WaitWhile(() => source.isPlaying);
        transition = false;
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    public IEnumerator GlitchAnim()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        MusicControl.Instance.StopMusic();
        while (true)
        {
            Camera.main.GetComponent<Kino.DigitalGlitch>().intensity += 0.01f;
            yield return new WaitForSecondsRealtime(0.025f);
        }
    }

}

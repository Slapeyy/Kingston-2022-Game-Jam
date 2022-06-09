using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControler : MonoBehaviour
{
    public static MenuControler Instance { get; private set; }

    public GameObject menuPanel;
    public GameObject optionsPanel;

    public AudioSource source;

    public AudioClip hoverSound;
    public AudioClip pressSound;

    public GameObject soundSlider;
    public GameObject musicSlider;

    public bool disableMenu = false;
    private bool gamePaused = false;

    public GameObject webglend;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        if (PlayerPrefs.HasKey("Sound"))
        {
            soundSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sound");
        }
        else
        {
            PlayerPrefs.SetFloat("Sound", 1.0f);
            soundSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sound");
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Music");
        }
        else
        {
            PlayerPrefs.SetFloat("Music", 1.0f);
            musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Music");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !disableMenu)
        {
            if(!gamePaused) Pause();
            else Resume();
        }
    }

    public void PlayHoover()
    {
        source.PlayOneShot(hoverSound, PlayerPrefs.GetFloat("Sound"));
    }

    public void PlayClick()
    {
        source.PlayOneShot(pressSound, PlayerPrefs.GetFloat("Sound"));
    }

    public void Pause()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().menuUp = true;
        Time.timeScale = 0.0f;
        menuPanel.SetActive(true);
        gamePaused = true;
    }

    public void SoundSlider()
    {
        PlayerPrefs.SetFloat("Sound", soundSlider.GetComponent<Slider>().value);
    }

    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("Music", musicSlider.GetComponent<Slider>().value);
    }

    public void Resume()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().menuUp = false;
        Time.timeScale = 1.0f;
        menuPanel.SetActive(false);
        gamePaused = false;
    }

    public void ToggleOptions()
    {
        optionsPanel.SetActive(!optionsPanel.activeInHierarchy);
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Time.timeScale = 1.0f;
#if UNITY_WEBGL

        webglend.SetActive(true);

#endif
        Application.Quit();
    }
}

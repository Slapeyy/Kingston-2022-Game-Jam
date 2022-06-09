using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace GameJam.Krzysztof
{
    public class SceneManager : MonoBehaviour
    {
        private bool disabledEv = false;
        public AudioClip quitSound;
        public AudioClip hoverSound;
        public AudioClip pressSound;
        public AudioClip glitchSound;
        public AudioSource source;
        public GameObject soundSlider;
        public GameObject musicSlider;
        public GameObject menu;
        public GameObject welcomeText;

        public GameObject settingsPanel;
        public GameObject scoresPanel;
        public GameObject menuPanel;
        public GameObject tutorialPanel;

        public GameObject monstersAmount;
        public GameObject bossesAmount;
        public GameObject corruptionsAmount;

        public GameObject textToTrans;
        public string welcomeString;
        public string glitchString;

        public VertexGradient glitchGradient;
        public VertexGradient normalGradient;

        public float delayBeforeSound;
        public float delayAfterSound;

        public GameObject webglend;
        private void Awake()
        {
            if (PlayerPrefs.HasKey("Sound"))
            {
                soundSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sound");
            }
            else
            {
                PlayerPrefs.SetFloat("Sound", 0.5f);
                soundSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sound");
            }

            if (PlayerPrefs.HasKey("Music"))
            {
                musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Music");
            }
            else
            {
                PlayerPrefs.SetFloat("Music", 0.5f);
                musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Music");
            }

            if (PlayerPrefs.HasKey("Monsters"))
            {
                monstersAmount.GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Monsters").ToString();
            }
            else
            {
                PlayerPrefs.SetInt("Monsters", 0);
                monstersAmount.GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Monsters").ToString();
            }
            if (PlayerPrefs.HasKey("Bosses"))
            {
                bossesAmount.GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Bosses").ToString();
            }
            else
            {
                PlayerPrefs.SetInt("Bosses", 0);
                bossesAmount.GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Bosses").ToString();
            }
            if (PlayerPrefs.HasKey("Corruptions"))
            {
                corruptionsAmount.GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Corruptions").ToString();
            }
            else
            {
                PlayerPrefs.SetInt("Corruptions", 0);
                corruptionsAmount.GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Corruptions").ToString();
            }
        }

        private void Start()
        {
            MusicControl.Instance.PlayMenuLoop();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void ToggleTutorial()
        {
            if (!disabledEv)
            {
                settingsPanel.SetActive(false);
                scoresPanel.SetActive(false);
                menuPanel.SetActive(false);
                tutorialPanel.SetActive(true);
            }
        }

        public void Return()
        {
            if (!disabledEv)
            {
                settingsPanel.SetActive(false);
                scoresPanel.SetActive(false);
                menuPanel.SetActive(true);
                tutorialPanel.SetActive(false);
            }
        }

        public void SoundSlider()
        {
            PlayerPrefs.SetFloat("Sound", soundSlider.GetComponent<Slider>().value);
        }

        public void MusicSlider()
        {
            PlayerPrefs.SetFloat("Music", musicSlider.GetComponent<Slider>().value);
        }


        public void PlayHoover()
        {
            if (!disabledEv) source.PlayOneShot(hoverSound, PlayerPrefs.GetFloat("Sound"));
        }

        public void PlayClick()
        {
            if (!disabledEv) source.PlayOneShot(pressSound, PlayerPrefs.GetFloat("Sound"));
        }

        private void DisableEverythingElse()
        {
            disabledEv = true;
        }

        public void PlayGame()
        {
            if (!disabledEv)
            {
                DisableEverythingElse();
                menu.SetActive(false);
                welcomeText.SetActive(true);
                textToTrans.GetComponent<TMP_Text>().colorGradient = normalGradient;
                textToTrans.GetComponent<TMP_Text>().text = welcomeString;
                StartCoroutine(GlitchApp());
            }
        }

        public void ToggleScores()
        {
            if (!disabledEv)
            {
                scoresPanel.SetActive(!scoresPanel.activeInHierarchy);
            }
        }

        public void ToggleSettings()
        {
            if (!disabledEv)
            {
                settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
            }
        }

        public void QuitApp()
        {
            if (!disabledEv)
            {
                DisableEverythingElse();
                StartCoroutine(QuitCor());
            }
        }

        public IEnumerator QuitCor()
        {
            source.PlayOneShot(quitSound, PlayerPrefs.GetFloat("Sound"));
            yield return new WaitWhile(() => source.isPlaying);

#if UNITY_WEBGL

            webglend.SetActive(true);

#endif

            Application.Quit();
        }

        public IEnumerator GlitchApp()
        {
            yield return new WaitForSeconds(delayBeforeSound);
            source.PlayOneShot(glitchSound, PlayerPrefs.GetFloat("Sound") * 0.4f);
            StartCoroutine(GlitchAnim());
            yield return new WaitWhile(() => source.isPlaying);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public IEnumerator GlitchAnim()
        {
            yield return new WaitForSeconds(delayAfterSound);
            textToTrans.GetComponent<TMP_Text>().colorGradient = glitchGradient;
            textToTrans.GetComponent<TMP_Text>().text = glitchString;
            MusicControl.Instance.StopMusic();
            while (true)
            {
                Camera.main.GetComponent<Kino.DigitalGlitch>().intensity += 0.02f;
                yield return new WaitForSeconds(0.025f);
            }
        }

        public void QuitEarly()
        {
            if (disabledEv)
            {
                Debug.Log("QuitEarly: Quit Early called");

#if UNITY_WEBGL

                webglend.SetActive(true);

#endif

                Application.Quit();
            }
        }
    }
}
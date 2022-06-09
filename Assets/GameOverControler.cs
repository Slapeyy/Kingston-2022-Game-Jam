using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverControler : MonoBehaviour
{
    public static GameOverControler Instance { get; private set; }

    public GameObject gameOverCanvas;
    private bool glitching = false;

    public int killedMonsters;
    public int killedBosses;
    public int seenCorruptions;

    public AudioClip glitch;
    public AudioSource source;

    public GameObject gameoverText;
    public GameObject monstersText;
    public GameObject bossesText;
    public GameObject corruptionsText;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameOver(bool won)
    {
        string wonText = "You beated Corruption... Gods thought it's impossible!";
        string loseText = "Corruption got the Better of You";

        gameoverText.GetComponent<TMP_Text>().text = won ? wonText : loseText;

        MenuControler.Instance.disableMenu = true;
        MusicControl.Instance.GameOverLoop();
        gameOverCanvas.SetActive(true);
        glitching = !won;

        StartCoroutine(RandomGlitches());
        if (killedMonsters > PlayerPrefs.GetInt("Monsters")) {
            monstersText.GetComponent<TMP_Text>().text += killedMonsters.ToString() + " <color=\"red\">New HIGHSCORE!</color>";
            PlayerPrefs.SetInt("Monsters",  killedMonsters);
        }
        else monstersText.GetComponent<TMP_Text>().text += killedMonsters.ToString();
        if (killedBosses > PlayerPrefs.GetInt("Bosses")) {
            bossesText.GetComponent<TMP_Text>().text += killedBosses.ToString() + " <color=\"red\">New HIGHSCORE!</color>";
            PlayerPrefs.SetInt("Bosses", killedBosses);
        }
        else bossesText.GetComponent<TMP_Text>().text += killedBosses.ToString();
        if (seenCorruptions > PlayerPrefs.GetInt("Corruptions"))
        {
            corruptionsText.GetComponent<TMP_Text>().text += seenCorruptions.ToString() + " <color=\"red\">New HIGHSCORE!</color>";
            PlayerPrefs.SetInt("Corruptions", seenCorruptions);
        }
        else corruptionsText.GetComponent<TMP_Text>().text += seenCorruptions.ToString();

    }

    public void MainMenu()
    {
        SceneTransitionManager.Instance.Transition(0);
    }

    private IEnumerator RandomGlitches()
    {
        while (glitching)
        {
            float randomTime = Random.Range(0.1f, 0.8f);
            float randomIntensity = Random.Range(0.1f, 0.9f);
            float randomCD = Random.Range(3f, 7f);

            Camera.main.GetComponent<Kino.DigitalGlitch>().intensity = randomIntensity;
            source.PlayOneShot(glitch, PlayerPrefs.GetFloat("Sound") * 0.3f);
            yield return new WaitForSeconds(randomTime);
            Camera.main.GetComponent<Kino.DigitalGlitch>().intensity = 0f;

            yield return new WaitForSeconds(randomCD);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    SpriteRenderer m_SR;
    Color m_OriginalColor;
    private GameObject m_Player;
    private int m_HitCount = 0;
    public GameObject DialoguePrefab;
    private AudioClip m_HeheBoiClip;
    private AudioClip m_HelloThereClip;
    private AudioClip m_BababooeyClip;
    private AudioSource m_AudioSource;

    private string[] W_Lines;
    private string[] A_Lines;
    private string[] S_Lines;
    private string[] D_Lines;
    void Start()
    {
        W_Lines = new string[3] { "Wah wah wee wah.", "Wah ha ha ha ha!", "Wah, wah, waaaaaah." };
        A_Lines = new string[3] { "Amoogus.", "AAAAAAAAAAAAAAAA!", "I love memes." };
        S_Lines = new string[3] { "Spooky, scary S key sends shivers down your spine!", "Some...BODY once told me.", "I'm sexy and I know it." };
        D_Lines = new string[3] { "Oh my god, I'm free!", "I won't be your prisoner any longer.", "Dis D key is no longer yours, buckaroo." };

        m_AudioSource = GetComponent<AudioSource>();
        m_BababooeyClip = Resources.Load<AudioClip>("bababooey");
        m_HeheBoiClip = Resources.Load<AudioClip>("Hehe boi");
        m_HelloThereClip = Resources.Load<AudioClip>("HelloThere");
        int rnd = Random.Range(0, 3);
        if (rnd == 0)
            m_AudioSource.PlayOneShot(m_HeheBoiClip, PlayerPrefs.GetFloat("Sound") * 3.0f);
        else if(rnd == 1)
            m_AudioSource.PlayOneShot(m_HelloThereClip, PlayerPrefs.GetFloat("Sound") * 3.0f);
        else
            m_AudioSource.PlayOneShot(m_BababooeyClip, PlayerPrefs.GetFloat("Sound") * 1.5f);

        string txt = "";
        Vector2 offset = new Vector2(-0.1f, 15.0f);
        if(name.Contains("W_Monster"))
        {
            txt = W_Lines[Random.Range(0, 3)];
        }
        else if (name.Contains("A_Monster"))
        {
            txt = A_Lines[Random.Range(0, 3)];
        }
        else if (name.Contains("D_Monster"))
        {
            txt = D_Lines[Random.Range(0, 3)];
        }
        else if (name.Contains("S_Monster"))
        {
            txt = S_Lines[Random.Range(0, 3)];
            offset = new Vector2(-0.1f, 1.0f);
        }
        Instantiate(DialoguePrefab).GetComponent<DialogueControl>().Init(txt, offset, gameObject);
        m_Player = GameObject.FindWithTag("Player");
        m_SR = GetComponent<SpriteRenderer>();
        m_OriginalColor = m_SR.color;
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_Player.transform.position, Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_HitCount > 25)
        {
            EventManager.Get().KillBoss(name);
            GameOverControler.Instance.killedBosses++;
            Destroy(gameObject);
        }
        if (collision.transform.CompareTag("Bullet"))
        {
            m_SR.color = m_OriginalColor;
            StopAllCoroutines();
            StartCoroutine(Flash());
            m_HitCount++;
        }
    }
    IEnumerator Flash()
    {
        m_SR.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        m_SR.color = m_OriginalColor;
    }
}

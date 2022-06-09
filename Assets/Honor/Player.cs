using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    Rigidbody2D m_RB;
    float m_Horizontal;
    float m_Vertical;
    float m_MoveLimiter = 0.7f;
    float BulletSpeed = 10.0f;
    public float RunSpeed = 20.0f;
    public GameObject BulletPrefab;
    private float m_PlayerHealth = 1.0f;
    private float m_CorruptionBar = 0.0f;
    private Image m_CorruptionBarImage;
    private Image m_MutationBarImage;
    private float m_MutationBar = 0.0f;
    private Image m_HealthBarImage;
    private bool m_IsDead = false;
    private GameObject m_HPText;
    private int m_EnemyKilled = 0;
    private AudioSource m_AudioSource;
    private AudioClip m_NegativeButton;
    private AudioClip m_HitSound;

    private bool m_IsWEnabled = true;
    private bool m_IsAEnabled = true;
    private bool m_IsSEnabled = true;
    private bool m_IsDEnabled = true;

    private bool DoOnce2 = true;
    private bool DoOnce = false;

    private bool firstCorruption = false;

    public GameObject k_objectiveObj;
    public string k_objective = "Objective: Survive";
    public string k_objectiveN = "Objective: Kill all Skeletons to save the Forest";

    public bool menuUp = false;

    Color RMB_DisabledColor;

    void Start()
    {
        RMB_DisabledColor = GameObject.Find("Canvas").transform.Find("KeyPanel").Find("KeyRMB").GetComponent<Image>().color;
        m_AudioSource = GetComponent<AudioSource>();
        m_NegativeButton = Resources.Load<AudioClip>("negative");
        m_HitSound = Resources.Load<AudioClip>("Hit");
        m_HPText = GameObject.Find("Canvas").transform.Find("HpText").gameObject;
        m_CorruptionBarImage = GameObject.Find("Canvas").transform.Find("CorruptionBar").GetComponent<Image>();
        m_MutationBarImage = GameObject.Find("Canvas").transform.Find("MutationBar").GetComponent<Image>();
        m_HealthBarImage = GameObject.Find("Canvas").transform.Find("HealthBar").GetComponent<Image>();
        m_RB = GetComponent<Rigidbody2D>();
        EventManager.Get().OnDisableKey += OnDisableKey;
        EventManager.Get().OnEnableKey += OnEnableKey;
        EventManager.Get().OnKillEnemy += OnKillEnemy;
        EventManager.Get().OnPlayHitSound += OnPlayHitSound;
        k_objectiveObj.GetComponent<TMP_Text>().text = k_objectiveN;
    }
    void Update()
    {
        m_Vertical = 0;
        m_Horizontal = 0;
        if (m_IsDead)
            return;

        if (m_PlayerHealth <= 0.0f)
        {
            m_IsDead = true;
            StartCoroutine(GameOver());
        }
        if (m_CorruptionBar >= 1.0f)
        {
            if (!firstCorruption)
            {
                MusicControl.Instance.PlayOminuousLoop();
                GlitchControl.Instance.StartGlitching();
                k_objectiveObj.GetComponent<TMP_Text>().text = k_objective;
                firstCorruption = true;
            }

            GameOverControler.Instance.seenCorruptions++;
            EventManager.Get().GetCorrupted();
            m_CorruptionBar = 0.0f;
        }
        m_CorruptionBar += Time.deltaTime * 0.03f;
        m_CorruptionBarImage.fillAmount = m_CorruptionBar;
        if (m_IsWEnabled && m_IsSEnabled && m_IsAEnabled && m_IsDEnabled && DoOnce)
        {
            DoOnce = false;
            k_objectiveObj.GetComponent<TMP_Text>().text = k_objectiveN;
            MusicControl.Instance.PlayHappyLoop();
            GlitchControl.Instance.StopGlitching();
            firstCorruption = false;
        }
        //Input Handling------
        if (Input.GetKey(KeyCode.W) && m_IsWEnabled)
        {
            m_Vertical = 1;
        }
        else if (Input.GetKey(KeyCode.S) && m_IsSEnabled)
        {
            m_Vertical = -1;
        }
        if (Input.GetKey(KeyCode.A) && m_IsAEnabled)
        {
            m_Horizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D) && m_IsDEnabled)
        {
            m_Horizontal = 1;
        }

        if (Input.GetKeyDown(KeyCode.W) && !m_IsWEnabled)
        {
            m_AudioSource.PlayOneShot(m_NegativeButton, PlayerPrefs.GetFloat("Sound") * 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.S) && !m_IsSEnabled)
        {
            m_AudioSource.PlayOneShot(m_NegativeButton, PlayerPrefs.GetFloat("Sound") * 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.A) && !m_IsAEnabled)
        {
            m_AudioSource.PlayOneShot(m_NegativeButton, PlayerPrefs.GetFloat("Sound") * 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.D) && !m_IsDEnabled)
        {
            m_AudioSource.PlayOneShot(m_NegativeButton, PlayerPrefs.GetFloat("Sound") * 0.5f);
        }

        //Facing Mouse Position------
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float lookAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        if (mousePosition.x - transform.position.x < 0.0f)
        {
            if (transform.localScale.x > 0.0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            if (transform.localScale.x < 0.0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (!menuUp)
        {
            //Shooting-----
            if (Input.GetMouseButtonDown(0))
            {
                GameObject bullet = Instantiate(BulletPrefab);
                Destroy(bullet, 5);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, lookAngle);

                var dir = mousePosition - bullet.transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(dir.x, dir.y).normalized * BulletSpeed;
            }
            if (m_MutationBar >= 0.999f)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    var colliders = Physics2D.OverlapCircleAll(transform.position, 15);
                    int ct = 0;
                    
                    foreach (var collider in colliders)
                    {
                        if (ct >= 20)
                            break;

                        if (collider.gameObject.GetComponent<Enemy>() != null)
                        {
                            m_MutationBar = 0.0f;
                            m_MutationBarImage.fillAmount = m_MutationBar;
                            GlitchControl.Instance.StartMutate();
                            m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Chicken"), PlayerPrefs.GetFloat("Sound") * 0.1f);
                            collider.gameObject.GetComponent<Enemy>().BecomeChicken();
                            GameObject.Find("Canvas").transform.Find("KeyPanel").Find("KeyRMB").GetComponent<Image>().color = RMB_DisabledColor;
                            ct++;
                        }
                        
                    }
                }
            }
        }
    }
    IEnumerator HPText()
    {
        m_HPText.SetActive(true);
        yield return new WaitForSeconds(2);
        m_HPText.SetActive(false);
    }
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        EventManager.Get().GameOver();
        GameOverControler.Instance.GameOver(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            m_CorruptionBar += 0.3f;
            m_PlayerHealth -= 0.1f;
            m_HealthBarImage.fillAmount = m_PlayerHealth;
            m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Roblox"), PlayerPrefs.GetFloat("Sound") * 2.3f);
        }
        
    }
    public void DamagePlayer(float damageNumber)
    {
        m_PlayerHealth -= damageNumber;
        Debug.Log(m_PlayerHealth);
    }
    void FixedUpdate()
    {
        if (m_Horizontal != 0 && m_Vertical != 0) // Check for diagonal movement
        {
            m_Horizontal *= m_MoveLimiter;
            m_Vertical *= m_MoveLimiter;
        }
        m_RB.velocity = new Vector2(m_Horizontal * RunSpeed, m_Vertical * RunSpeed);
    }
    void OnKillEnemy()
    {
        m_EnemyKilled++;
        GameOverControler.Instance.killedMonsters++;
        if(m_MutationBar <= 1.01f)
        {
            m_MutationBar += 0.05f;
            m_MutationBarImage.fillAmount = m_MutationBar;

        }
        else
        {
            GameObject.Find("Canvas").transform.Find("KeyPanel").Find("KeyRMB").GetComponent<Image>().color = Color.white;
        }
        if(m_EnemyKilled >= 10 && m_PlayerHealth <= 0.9f)
        {
            StartCoroutine(HPText());
            m_EnemyKilled = 0;
            m_PlayerHealth += 0.1f;
            m_HealthBarImage.fillAmount = m_PlayerHealth;
        }
    }
    void OnDisableKey(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.W:
                m_IsWEnabled = false;
                DoOnce = true;
                break;
            case KeyCode.A:
                m_IsAEnabled = false;
                DoOnce = true;
                break;
            case KeyCode.S:
                m_IsSEnabled = false;
                DoOnce = true;
                break;
            case KeyCode.D:
                m_IsDEnabled = false;
                DoOnce = true;
                break;
        }
    }
    void OnEnableKey(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.W:
                m_IsWEnabled = true;
                break;
            case KeyCode.A:
                m_IsAEnabled = true;
                break;
            case KeyCode.S:
                m_IsSEnabled = true;
                break;
            case KeyCode.D:
                m_IsDEnabled = true;
                break;
        }
    }
    void OnPlayHitSound()
    {
        m_AudioSource.PlayOneShot(m_HitSound, PlayerPrefs.GetFloat("Sound") * 0.4f);
    }
}

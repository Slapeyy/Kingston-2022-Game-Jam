using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class GameManager : MonoBehaviour
{

    private bool m_WCorrupted;
    private bool m_ACorrupted;
    private bool m_SCorrupted;
    private bool m_DCorrupted;

    public GameObject KeyPanel;
    private List<KeyCode> m_Keys;
    public Sprite Corrupted_W;
    public Sprite Corrupted_A;
    public Sprite Corrupted_S;
    public Sprite Corrupted_D;
    public Sprite Pressed_W;
    public Sprite Pressed_A;
    public Sprite Pressed_S;
    public Sprite Pressed_D;
    public Sprite Pressed_LMB;
    public Sprite Pressed_RMB;
    private Sprite Normal_W;
    private Sprite Normal_A;
    private Sprite Normal_S;
    private Sprite Normal_D;
    private Sprite Normal_LMB;
    public static float s_KeyDisableTime = 4;
    public GameObject EnemyPrefab;
    private float m_EnemySpawnTimer = 0.5f;
    private int m_CurrentEnemyCount = 0;
    public int m_MaxEnemyCount = 50;
    public GameObject[] BossList;
    private float m_Timer = 300;
    private float m_DifficultyIncreaseTimer = 20.0f;
    private TextMeshProUGUI m_TimerText;
    private bool DoOnce = true;
    void Start()
    {
#if UNITY_WEBGL
        var cursorTex = Resources.Load<Texture2D>("Crosshair");
        float cursorWidth = cursorTex.width/2;
        float cursorHeight = cursorTex.height/2;
        Cursor.SetCursor(cursorTex, new Vector2(cursorWidth, cursorHeight), CursorMode.Auto);
#else
        Cursor.SetCursor(Resources.Load<Texture2D>("Crosshair"), new Vector2(256, 256), CursorMode.Auto);
#endif
        Normal_W = KeyPanel.transform.Find("KeyW").GetComponent<Image>().sprite;
        Normal_A = KeyPanel.transform.Find("KeyA").GetComponent<Image>().sprite;
        Normal_S = KeyPanel.transform.Find("KeyS").GetComponent<Image>().sprite;
        Normal_D = KeyPanel.transform.Find("KeyD").GetComponent<Image>().sprite;
        Normal_LMB = KeyPanel.transform.Find("KeyLMB").GetComponent<Image>().sprite;

        m_TimerText = GameObject.Find("Canvas").transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        m_Keys = new List<KeyCode>();
        m_Keys.Add(KeyCode.W);
        m_Keys.Add(KeyCode.A);
        m_Keys.Add(KeyCode.S);
        m_Keys.Add(KeyCode.D);
        EventManager.Get().OnGetCorrupted += OnGetCorrupted;
        EventManager.Get().OnKillEnemy += OnKillEnemy;
        EventManager.Get().OnKillBoss += OnKillBoss;
        EventManager.Get().OnGameOver += OnGameOver;

        MusicControl.Instance.PlayHappyLoop();
        Enemy.m_MovementSpeed = 1.0f;
        GameOverControler.Instance.killedBosses = 0;
        GameOverControler.Instance.killedMonsters = 0;
        GameOverControler.Instance.seenCorruptions = 0;
    }

    void Update()
    {
        if(m_Timer <= 0.0f && DoOnce)
        {
            DoOnce = false;
            GameOverControler.Instance.GameOver(true);
            return;
        }
        if (!m_WCorrupted)
            Normal_W = KeyPanel.transform.Find("KeyW").GetComponent<Image>().sprite = Normal_W;
        if (!m_ACorrupted)
            Normal_A = KeyPanel.transform.Find("KeyA").GetComponent<Image>().sprite = Normal_A;
        if (!m_SCorrupted)
            Normal_S = KeyPanel.transform.Find("KeyS").GetComponent<Image>().sprite = Normal_S;
        if (!m_DCorrupted)
            Normal_D = KeyPanel.transform.Find("KeyD").GetComponent<Image>().sprite = Normal_D;
        KeyPanel.transform.Find("KeyLMB").transform.GetComponent<Image>().sprite = Normal_LMB;

        if (Input.GetKey(KeyCode.W) && !m_WCorrupted)
        {
            KeyPanel.transform.Find("KeyW").GetComponent<Image>().sprite = Pressed_W; 
        }
        if(Input.GetKey(KeyCode.A) && !m_ACorrupted)
        {
            KeyPanel.transform.Find("KeyA").GetComponent<Image>().sprite = Pressed_A;
        }
        if (Input.GetKey(KeyCode.S) && !m_SCorrupted)
        {
            KeyPanel.transform.Find("KeyS").GetComponent<Image>().sprite = Pressed_S;
        }
        if (Input.GetKey(KeyCode.D) && !m_DCorrupted)
        {
            KeyPanel.transform.Find("KeyD").GetComponent<Image>().sprite = Pressed_D;
        }
        if(Input.GetMouseButton(0))
        {
            KeyPanel.transform.Find("KeyLMB").transform.GetComponent<Image>().sprite = Pressed_LMB;
        }
        m_Timer -= Time.deltaTime;
        m_DifficultyIncreaseTimer -= Time.deltaTime;
        if(m_DifficultyIncreaseTimer <= 0.0f)
        {
            m_DifficultyIncreaseTimer = 20.0f;
            EventManager.Get().IncreaseDifficulty();
            m_MaxEnemyCount += 10;
        }
        m_EnemySpawnTimer -= Time.deltaTime;
        m_TimerText.text = TimeSpan.FromSeconds(m_Timer).ToString(@"mm\:ss");
        if (m_EnemySpawnTimer <= 0.0f && m_CurrentEnemyCount <= m_MaxEnemyCount)
        {
            var point = RandomPointOnCircleEdge(35);
            Instantiate(EnemyPrefab, point, Quaternion.identity);
            m_EnemySpawnTimer = 0.5f;
            m_CurrentEnemyCount++;
        }
    }
    private Vector2 RandomPointOnCircleEdge(float radius)
    {
        return UnityEngine.Random.insideUnitCircle.normalized * radius;
    }
    Vector3 RandomPointOnXZCircle(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }
    void OnGetCorrupted()
    {
        if (m_Keys.Count <= 0)
            return;
        int random = UnityEngine.Random.Range(0, m_Keys.Count);
        KeyCode keyCode = m_Keys[random];
        m_Keys.Remove(keyCode);
        Debug.Log(keyCode);
        EventManager.Get().DisableKey(keyCode);
        CorruptKey(keyCode);
        var point = RandomPointOnCircleEdge(30);
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        Vector3 pointTest;
        do
        {
            pointTest = RandomPointOnXZCircle(playerPos, 17);

        } while (Vector3.Distance(pointTest, playerPos) < 2.0f);
        switch (keyCode)
        {
            case KeyCode.A:
                Instantiate(BossList[1], pointTest, Quaternion.identity);
                break;
            case KeyCode.S:
                Instantiate(BossList[2], pointTest, Quaternion.identity);
                break;
            case KeyCode.D:
                Instantiate(BossList[3], pointTest, Quaternion.identity);
                break;
            case KeyCode.W:
                Instantiate(BossList[0], pointTest, Quaternion.identity);
                break;
        }
        
    }
    void CorruptKey(KeyCode keyCode)
    {
        switch(keyCode)
        {
            case KeyCode.A:
                KeyPanel.transform.Find("KeyA").GetComponent<Image>().sprite = Corrupted_A;
                m_ACorrupted = true;
                break;
            case KeyCode.S:
                KeyPanel.transform.Find("KeyS").GetComponent<Image>().sprite = Corrupted_S;
                m_SCorrupted = true;
                break;
            case KeyCode.D:
                KeyPanel.transform.Find("KeyD").GetComponent<Image>().sprite = Corrupted_D;
                m_DCorrupted = true;
                break;
            case KeyCode.W:
                KeyPanel.transform.Find("KeyW").GetComponent<Image>().sprite = Corrupted_W;
                m_WCorrupted = true;
                break;
        }
    }
    void OnKillBoss(string name)
    {
        if(name.Contains("A_Monster"))
        {
            KeyPanel.transform.Find("KeyA").GetComponent<Image>().sprite = Normal_A;
            m_Keys.Add(KeyCode.A);
            m_ACorrupted = false;
            EventManager.Get().EnableKey(KeyCode.A);
        }
        else if(name.Contains("S_Monster"))
        {
            KeyPanel.transform.Find("KeyS").GetComponent<Image>().sprite = Normal_S;
            m_Keys.Add(KeyCode.S);
            m_SCorrupted = false;
            EventManager.Get().EnableKey(KeyCode.S);
        }
        else if(name.Contains("W_Monster"))
        {
            KeyPanel.transform.Find("KeyW").GetComponent<Image>().sprite = Normal_W;
            m_Keys.Add(KeyCode.W);
            m_DCorrupted = false;
            EventManager.Get().EnableKey(KeyCode.W);
        }
        else if(name.Contains("D_Monster"))
        {
            KeyPanel.transform.Find("KeyD").GetComponent<Image>().sprite = Normal_D;
            m_Keys.Add(KeyCode.D);
            m_WCorrupted = false;
            EventManager.Get().EnableKey(KeyCode.D);
        }
    }
    void OnGameOver()
    {
        Time.timeScale = 0;
    }
    void OnKillEnemy()
    {
        m_CurrentEnemyCount--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer m_SR;
    Color m_OriginalColor;
    private GameObject m_Player;
    private int m_HitCount = 0;
    private Animator m_Animator;
    private bool m_Move = true;
    private Collider2D m_Collider;
    public static float m_MovementSpeed = 1;
    private bool m_Run = false;

    public RuntimeAnimatorController ChickenAnimatorController;
    void Start()
    {
        m_Collider = GetComponent<Collider2D>();
        m_Animator = gameObject.GetComponent<Animator>();
        m_Player = GameObject.FindWithTag("Player");
        m_SR = GetComponent<SpriteRenderer>();
        m_OriginalColor = m_SR.color;

        EventManager.Get().OnIncreaseDifficulty += OnIncreaseDifficulty;
    }
    private void OnDestroy()
    {
        EventManager.Get().OnIncreaseDifficulty -= OnIncreaseDifficulty;
    }
    private void Update()
    {
        if(m_Run)
        {
            Vector2 runDir = transform.position - m_Player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, runDir * 100, Time.deltaTime * 5);
            return;
        }
        if(m_Move)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            float lookAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

            if (m_Player.transform.position.x - transform.position.x < 0.0f)
            {
                if (transform.localScale.x > 0.0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                if (transform.localScale.x < 0.0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            transform.position = Vector2.MoveTowards(transform.position, m_Player.transform.position, Time.deltaTime * m_MovementSpeed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.transform.CompareTag("Bullet"))
        {
            m_SR.color = m_OriginalColor;
            StopAllCoroutines();
            StartCoroutine(Flash());
            m_HitCount++;
            if (m_HitCount > 2)
            {
                m_Collider.enabled = false;
                m_Move = false;
                EventManager.Get().KillEnemy();
                m_Animator.SetTrigger("Die");
            }
        }
    }
    void OnIncreaseDifficulty()
    {
        m_MovementSpeed += 0.01f;
    }
    public void GetDestroyed()
    {
        Destroy(gameObject);
    }
    public void BecomeChicken()
    {
        m_Animator.runtimeAnimatorController = ChickenAnimatorController;
        m_Collider.enabled=false;
        m_SR.color = Color.white;
        m_Run = true;
        Destroy(gameObject, 5);
    }
        
    IEnumerator Flash()
    {
        m_SR.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        m_SR.color = m_OriginalColor;
    }
}

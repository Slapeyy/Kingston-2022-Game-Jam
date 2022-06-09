using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventManager : MonoBehaviour
{
    private static EventManager s_Instance;
    public event Action<KeyCode> OnDisableKey;
    public event Action<KeyCode> OnEnableKey;
    public event Action OnGetCorrupted;
    public event Action OnKillEnemy;
    public event Action<string> OnKillBoss;
    public event Action OnGameOver;
    public event Action OnIncreaseDifficulty;
    public event Action OnPlayHitSound;
    private void Awake()
    {
        s_Instance = this;
    }
    public void DisableKey(KeyCode keyCode)
    {
        OnDisableKey?.Invoke(keyCode);
    }
    public void EnableKey(KeyCode keycode)
    {
        OnEnableKey?.Invoke(keycode);
    }
    public void GetCorrupted()
    {
        OnGetCorrupted?.Invoke();
    }
    public void KillEnemy()
    {
        OnKillEnemy?.Invoke();
    }
    public void KillBoss(string name)
    {
        OnKillBoss?.Invoke(name);
    }
    public void GameOver()
    {
        OnGameOver?.Invoke();
    }
    public void IncreaseDifficulty()
    {
        OnIncreaseDifficulty?.Invoke();
    }
    public void PlayHitSound()
    {
        OnPlayHitSound?.Invoke();
    }
    public static EventManager Get()
    {
        return s_Instance;
    }
}

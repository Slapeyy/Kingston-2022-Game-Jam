using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Krzysztof;

public class Bullet : MonoBehaviour
{
    public float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            EventManager.Get().PlayHitSound();
            Destroy(gameObject);
        }
    }
}

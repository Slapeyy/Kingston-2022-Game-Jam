using UnityEngine;
using System.Collections;

namespace GameJam.Krzysztof
{
    public class AttackCollider : MonoBehaviour
    {
        CircleCollider2D attackCollider;
        Coroutine attackCor;
        float damage;

        // Awake is called before every Start
        private void Awake()
        {
            attackCollider = GetComponent<CircleCollider2D>();
            attackCollider.enabled = false;
        }

        public void SetCollider(float range, float dmg)
        {
            damage = dmg;
            attackCollider.enabled = true;
            attackCollider.radius = range * 25;
            if (attackCor == null) attackCor = StartCoroutine(AttackCor());
            else
            {
                StopCoroutine(attackCor);
                attackCor = StartCoroutine(AttackCor());
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
            {
                Debug.Log("Attacked");
                other.transform.root.GetComponent<Player>().DamagePlayer(damage);
            }
        }

        private IEnumerator AttackCor()
        {
            yield return new WaitForSeconds(0.1f);
            attackCollider.enabled = false;
        }
    }
}
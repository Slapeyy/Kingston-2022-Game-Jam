using System.Collections;
using UnityEngine;

namespace GameJam.Krzysztof
{
    public class BaseMonster : Monster
    {
        #region Private Serialized Attributes

        [Header("Info")]

        /// <summary>
        /// Monster's target for attacking
        /// </summary>
        [Tooltip("Monster's target for attacking")]
        [SerializeField] private GameObject target;

        /// <summary>
        /// Layer mask containing obstacles
        /// </summary>
        [Tooltip("Layer mask containing obstacles")]
        [SerializeField] private LayerMask obstaclesMask;

        [Header("Statistics")]

        /// <summary>
        /// Monster base parameters
        /// </summary>
        [Tooltip("Monster base parameters")]
        [SerializeField] private MonsterStatistics stats;

        #endregion

        #region Private Attributes

        #region Stats

        /// <summary>
        /// Current health of the monster, starts with base health and gets reduced everytime monster is damaged. 
        /// Can be increased with health regen
        /// </summary>
        private float health;

        private Coroutine healthRegenerationCor;

        #endregion

        #region Components Calls

        /// <summary>
        /// Animator attached to the monster
        /// </summary>
        private Animator anim;

        /// <summary>
        /// Collider used for attacks
        /// </summary>
        private AttackCollider aCollider;

        #endregion

        #region Coroutines

        private Coroutine flashCor;

        #endregion

        #endregion

        #region MonoBehaviour Callbacks

        /// <summary>
        /// Awake is called before any Start is called. All the calls to components are in Awake.
        /// </summary>
        private void Awake()
        {
            anim = GetComponent<Animator>();
            aCollider = transform.Find("Attack").GetComponent<AttackCollider>();
            target = GameObject.FindWithTag("Player");
        }

        // Start is called before the first frame update
        private void Start()
        {
            health = stats.maxHealth;
            anim.SetFloat("Health", health);
            healthRegenerationCor = StartCoroutine(HealthRegenCoroutine());
        }

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion

        #region Public Methods

        #region Offence

        public override void Attack()
        {
            aCollider.SetCollider(stats.attackRange, stats.attackDamage);
        }

        public override void SpecialAttack()
        {
            aCollider.SetCollider(stats.specialRange, stats.specialAttackDamage);
        }

        #endregion

        #region Defence 

        public override void Damage(float amount)
        {
            //Randomise damage a bit
            float randFrac = Random.Range(0.8f, 1.2f);

            //Set health to be less than damage
            health -= randFrac * amount;

            anim.SetFloat("Health", health);

            //Flash
            if(flashCor != null) StopCoroutine(flashCor);
            flashCor = StartCoroutine(Flash());
        }

        public override void Knockback(float knockBackForce, Transform forcePosition)
        {
            //Get the relative position of the force to the position of the monster
            Vector2 relativeForcePosition = (forcePosition.position - transform.position).normalized;
            
            //Get the knockback direction which should be opposite to relative position of the force
            Vector2 knockbackDirection = new Vector2(-relativeForcePosition.x, -relativeForcePosition.y);

            //Get knockback vector which is combination of knockback direction and konckback force
            Vector3 knockback = new Vector3(knockbackDirection.x * knockBackForce, knockbackDirection.y * knockBackForce, 0);
            
            //Use raycasting to check whether when konckedback monster will meet the wall or not (if yes perhaps stun the monster)
            Ray2D ray = new Ray2D(transform.position, knockbackDirection);

            Debug.DrawRay(ray.origin, ray.direction * knockback.magnitude, Color.red, 2f);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, knockback.magnitude, obstaclesMask);
            if (hit.collider != null)
            {
                //travels up to collider distance and randomises stun duration
                transform.position = hit.point;
                float stunnedQ = Random.Range(0, 1f);
                if(stunnedQ > 0.2f)
                {
                    float randDur = Random.Range(0, 2f);
                    Stun(randDur);
                }
            }
            else
            {
                //travels full knockback distance
                transform.position += knockback;
            }
        }

        public override void Stun(float stunDuration)
        {
            //Randomise stun a bit
            float randFrac = Random.Range(0.8f, 1.2f);

            //Get current stun duration
            float currentStun = anim.GetFloat("Stun");
            
            //If current stun duration is less than new stun, set stun duration to new stun, otherwise remain null
            if(currentStun < stunDuration * randFrac) anim.SetFloat("Stun", stunDuration * randFrac);

            Debug.Log("BaseMonster: Monster got Stunned");
        }

        public override void Death()
        {
            //Called at the end of Death animation which occurs when Health is 0
            StopAllCoroutines();

            Destroy(gameObject);
        }

        #endregion

        #region Getters

        public override GameObject GetTarget()
        {
            return target;
        }

        public override MonsterStatistics GetStatistics()
        {
            return stats;
        }

        #endregion

        #endregion

        #region Private Methods 

        private IEnumerator HealthRegenCoroutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(1);

                health += stats.healthRegeneration;

                if(health > stats.maxHealth) health = stats.maxHealth;
            }
        }

        private IEnumerator Flash()
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        #endregion
    }
}
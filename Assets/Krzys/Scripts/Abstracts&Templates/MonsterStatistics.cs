using UnityEngine;

namespace GameJam.Krzysztof
{
    [CreateAssetMenuAttribute(menuName = "Monsters/Statistics")]
    public class MonsterStatistics : ScriptableObject
    {
        #region Health

        [Header("Health")]
        
        /// <summary>
        /// Maximum Health for the Monster
        /// </summary>
        [Tooltip("Maximum Health for the Monster")]
        public float maxHealth;

        /// <summary>
        /// Amount of Health Monster regains each second (still cannot excess max health)
        /// </summary>
        [Tooltip("Amount of Health Monster regains each second (still cannot excess max health)")]
        public float healthRegeneration;

        #endregion

        #region Movement

        [Header("Movement")]

        /// <summary>
        /// Maximum movement speed for the Monster
        /// </summary>
        [Tooltip("Maximum movement speed for the Monster")]
        public float movementSpeed;

        /// <summary>
        /// Maximum rotation speed for the Monster
        /// </summary>
        [Tooltip("Speed at which Monster rotates")]
        public float rotationSpeed;

        /// <summary>
        /// How fast Monster accelerates
        /// </summary>
        [Tooltip("How fast Monster accelerates")]
        public float acceleration;

        #endregion

        #region Attack

        [Header("Attack")]

        /// <summary>
        /// Amount of damage Monster deals
        /// </summary>
        [Tooltip("Amount of damage Monster deals")]
        public float attackDamage;

        /// <summary>
        /// Number of attack per second (2 - attack every 0.5 second)
        /// </summary>
        [Tooltip("Number of attack per second (2 - attack every 0.5 second)")]
        public float attackSpeed;

        /// <summary>
        /// How far can monster reach with their basic attack
        /// </summary>
        [Tooltip("How far can monster reach with their basic attack")]
        public float attackRange;

        /// <summary>
        /// Amount of damage Monster deals with special attack
        /// </summary>
        [Tooltip("Amount of damage Monster deals with special attack")]
        public float specialAttackDamage;

        /// <summary>
        /// Amount of Special attacks per all attacks
        /// </summary>
        [Tooltip("Amount of Special attacks per all attacks (1 - Every attack is special, 0.5 - Every second attack is special)")]
        [Range(0f, 1f)]
        public float specialAttackPerAttack;

        /// <summary>
        /// How far can monster reach with their special attack
        /// </summary>
        [Tooltip("How far can monster reach with their special attack")]
        public float specialRange;

        #endregion

    }
}
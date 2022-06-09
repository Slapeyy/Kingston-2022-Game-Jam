using UnityEngine;

namespace GameJam.Krzysztof
{
    public abstract class Monster : MonoBehaviour
    {
        #region Public Methods

        /// <summary>
        /// Script used to deal damage to the monster
        /// Monster must be able to receive damage
        /// </summary>
        /// <param name="amount">Amount of damage player gets</param>
        public abstract void Damage(float amount);

        /// <summary>
        /// Script used to clear up the monster
        /// Monster must be able to die
        /// </summary>
        public abstract void Death();

        /// <summary>
        /// Scipt used to communicate the target of the monster
        /// Monster must know what he is chasing
        /// </summary>
        /// <returns>Target</returns>
        public abstract GameObject GetTarget();

        /// <summary>
        /// Script used to communicate monster's parameters
        /// All Monster have set of variables that define them
        /// </summary>
        /// <returns>Monster's Parameters, including attack damage, health etc.</returns>
        public abstract MonsterStatistics GetStatistics();

        /// <summary>
        /// Script used for basic attacks
        /// All monsters should be able to attack
        /// </summary>
        public abstract void Attack();
        
        /// <summary>
        /// Script used for special attacks
        /// Some monsters can use special attack
        /// </summary>
        public abstract void SpecialAttack();

        /// <summary>
        /// Script used to send monster away from the player
        /// Monsters can be knocked back so player can handle them easier
        /// </summary>
        /// <param name="knockBackForce">Distance by which the monster will be knocked back</param>
        /// <param name="forcePosition">Position of attacker when attacking</param>
        public abstract void Knockback(float knockBackForce, Transform forcePosition);

        /// <summary>
        /// Script used to disable all actions of the monster for set period of time
        /// Player should have chance to stun the monster
        /// </summary>
        /// <param name="stunDuration">Time the monster is stunned for</param>
        public abstract void Stun(float stunDuration);

        #endregion
    }
}

using UnityEngine;

namespace GameJam.Krzysztof
{
    public class Special : StateMachineBehaviour
    {
        Monster monster;
        MonsterStatistics stats;
        float attackTime;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            monster = animator.GetComponent<Monster>();
            stats = monster.GetStatistics();
            attackTime = 1 / stats.attackSpeed;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            attackTime -= Time.deltaTime;
            if (attackTime <= 0) animator.SetBool("Attack", false);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat("SpecialCharge", 0f);
            monster.SpecialAttack();
        }
    }
}
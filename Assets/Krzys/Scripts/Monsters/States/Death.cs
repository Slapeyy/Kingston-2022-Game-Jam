using UnityEngine;

namespace GameJam.Krzysztof
{
    public class Death : StateMachineBehaviour
    {
        Monster monster;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Get all information about the Monster
            monster = animator.GetComponent<Monster>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Wait for Monster animation to play out
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Call monster death script destroying the game object etc.
            monster.Death();
        }
    }
}
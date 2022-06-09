using UnityEngine;
using System.Collections;

namespace GameJam.Krzysztof
{
    public class Chase : StateMachineBehaviour
    {
        Monster monster;
        GameObject target;
        Transform targetTransform;
        MonsterStatistics stats;
        Transform transform;
        Coroutine movementCoroutine;
        Vector3 targetPosition;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            monster = animator.GetComponent<Monster>();
            target = monster.GetTarget();
            stats = monster.GetStatistics();
            transform = animator.transform;
            targetTransform = target.transform;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Check if close enough to attack
            bool distance = animator.GetFloat("SpecialCharge") < 0.999
                ? Vector2.Distance(monster.transform.position, target.transform.position) < stats.attackRange
                : Vector2.Distance(monster.transform.position, target.transform.position) < stats.specialRange;
            if (distance)
            {
                animator.SetTrigger("Attack");
            }
            else
            {
                //Update target position
                UpdatePosition();
            }
            
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        private void UpdatePosition()
        {
            Vector2 direction = new Vector2(targetTransform.position.x, targetTransform.position.y);
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            direction = (direction - position).normalized;
            targetPosition = transform.position + new Vector3(direction.x, direction.y) * stats.movementSpeed * Time.deltaTime;
            transform.position = targetPosition;
        }
    }
}
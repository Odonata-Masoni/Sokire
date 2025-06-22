using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{
    public string boolName;
    public bool updateOnState;
    public bool updateOnStateMachine;
    public bool valueOnEnter, valueOnExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            animator.SetBool(boolName, valueOnEnter);
            if (boolName == AnimationStrings.canMove)
            {
                animator.SetBool(AnimationStrings.canMove, valueOnEnter);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            animator.SetBool(boolName, valueOnExit);
            if (boolName == AnimationStrings.canMove)
            {
                animator.SetBool(AnimationStrings.canMove, valueOnExit);
            }
        }
    }

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
            animator.SetBool(boolName, valueOnEnter);
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool(boolName, valueOnExit);
    }
}
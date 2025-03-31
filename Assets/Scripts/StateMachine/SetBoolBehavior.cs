using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolBehavior : StateMachineBehaviour
{
    public string boolName;
    // Varsay�lan olarak false olduklar� i�in biz de�er atamas� yapmad���m�z s�rece bir �ey olmayacak
    public bool updateOnState;
    public bool updateOnStateMachine;
    public bool valueOnEnter, valueOnExit;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Bu bool de�eri sadece state'de g�ncelleme yapt���m�zda de�i�ecek
        if (updateOnState)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Bu bool de�eri sadece state'de g�ncelleme yapt���m�zda de�i�ecek
        if (updateOnState)
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // Bu bool de�eri sadece state machine'de g�ncelleme yapt���m�zda de�i�ecek
        if (updateOnStateMachine)
            animator.SetBool(boolName, valueOnEnter);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        // Bu bool de�eri sadece state machine'de g�ncelleme yapt���m�zda de�i�ecek
        if (updateOnStateMachine)
            animator.SetBool(boolName, valueOnExit);
    }
}

using System.Collections;
using UnityEngine;

public class MovementStateMachine
{
    private IState currentlyRunningState = null;
    private IState previousState = null;


    public void ChangeState(IState newState)
    {
        if(this.currentlyRunningState != null)
        {
            this.currentlyRunningState.Exit();
        }

        this.previousState = currentlyRunningState;

        this.currentlyRunningState = newState;
        this.currentlyRunningState.Enter();
    }

    public void ExecuteStateUpdate()
    {
        if(this.currentlyRunningState != null)
        {
            this.currentlyRunningState.Execute();
        }
    }

    public void ExecuteStateFixedUpdate()
    {
        if(this.currentlyRunningState != null)
        {
            this.currentlyRunningState.FixedExecute();
        }
    }

    public void SwitchToPreviousState()
    {
        this.currentlyRunningState.Exit();
        this.currentlyRunningState = this.previousState;
        this.currentlyRunningState.Enter();
    }

} 

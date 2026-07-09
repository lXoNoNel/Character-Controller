

using System;
using UnityEngine;
using MainGame.Movement.States;

public class MovementStatemachineHandler
{
    MovementHandler movementHandler;


    MovementStateMachine movementStateMachine;
    private Action<float> HandleMovement;
    private Action HandleRotation;
    public PlayerData data;



    public MovementStatemachineHandler(Action<float> HandleMovement, Action HandleRotation, MovementHandler movementHandler, MovementStateMachine movementStateMachine, PlayerData data) {
        this.HandleMovement = HandleMovement;
        this.HandleRotation = HandleRotation;
        this.movementHandler = movementHandler;
        this.movementStateMachine = movementStateMachine;
        this.data = data;
        this.HandleRotation = HandleRotation;
    }


    public void ChangeRootState(IState movementState)
    {
        this.movementStateMachine.ChangeState(movementState);
    }


    //----------------------------------------------------------

    #region ENUM STATE DECLERATION

    public void CallbackChangeStateInObj(MovementStateEnum newState)
    {
        data.movementState = newState;
    }

    #endregion

    //----------------------------------------------------------


    public void finalStart()
    {
        locomotionStart();
    }

    public void finalUpdate()
    {
        locomotionUpdate();
    }

//--------------------------------------------------

    #region LOCOMOTION CALLS

    public void locomotionStart()
    {
        movementStateMachine.ChangeState(new BasicLocomotion(movementHandler, ChangeRootState, CallbackChangeStateInObj, HandleMovement, HandleRotation, data, BasicLocomotionStateEnum.Idle));
    }

    public void locomotionUpdate()
    {
        movementStateMachine.ExecuteStateUpdate();
    }

    #endregion


//--------------------------------------------------


//HANDLE INTER PARENTAL TRANSITIONS
    void handleTransitions()
    {
        
    }
}
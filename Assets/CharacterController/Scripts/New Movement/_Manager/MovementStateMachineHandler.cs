

using System;
using UnityEngine;
using MainGame.Movement.States;
using Unity.Android.Gradle.Manifest;

public class MovementStatemachineHandler
{
    public MovementHandler movementHandler;


    public MovementStateMachine movementStateMachine;
    public Action<float> HandleMovement;
    public System.Action HandleRotation;
    public System.Action activeFallingHandling;

    public PlayerGroundCheck playerGroundCheck;
    public bool isPlayerObjGrounded;

    public PlayerData data;

    public xPlayerPrimaryState exactCurrentState;


 
    public MovementStatemachineHandler(MovementHandler movementHandler,
    MovementStateMachine movementStateMachine) {
        this.movementHandler = movementHandler;

        this.HandleMovement = movementHandler.HandleMovement;
        this.activeFallingHandling = movementHandler.activeFallingHandling;
        this.HandleRotation = movementHandler.HandleRotation;
        this.movementStateMachine = movementStateMachine;
        this.playerGroundCheck = movementHandler.playerGroundCheck;
        this.data = movementHandler.data;
        this.HandleRotation = movementHandler.HandleRotation;
    }


    public void ChangeRootState(IState movementState)
    {
        this.movementStateMachine.ChangeState(movementState);
    }


    //----------------------------------------------------------

    #region ENUM STATE DECLERATION

    public void CallbackChangeStateInObj(xPlayerPrimaryState newState)
    {
        data.exactMovementState = newState;
    }

    #endregion

    //----------------------------------------------------------


    public void finalStart()
    {
        locomotionStart();

        exactCurrentState = data.exactMovementState;
    }

    public void finalFixedUpdate()
    {
        locomotionUpdate();
    }

    public void finalUpdate()
    {
        exactCurrentState = data.exactMovementState;
        isPlayerObjGrounded = playerGroundCheck.isPlayerObjGrounded();

        handleTransitions();

    }

//--------------------------------------------------

    #region LOCOMOTION CALLS

    public void locomotionStart()
    {
        movementStateMachine.ChangeState(new BasicLocomotion(movementHandler, ChangeRootState, 
        CallbackChangeStateInObj, xPlayerPrimaryState.Movement.Locomotion.Idle));
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
        switch (exactCurrentState)
        {
            // Is it ANY locomotion leaf node? (Idle, Walking, Running, Sprinting)
            // Note: You would just need to add ', IMovementSubState' to LocomotionLeaf
            // if you want to use this exact syntax cleanly across both.
            case var s when s.IsChildOf(xPlayerPrimaryState.Movement.Locomotion):
                if(!isPlayerObjGrounded)
                {
                    movementStateMachine.ChangeState(new Airborne(movementHandler, this,
                    xPlayerPrimaryState.Movement.Airborne.Falling));
                }
                break;

            // Is it ANY airborne leaf node? (Grounded, Jumping, Falling, Landing)
            case IAirborneSubState:
                // movementStateMachine.ChangeState(new WalkingLocomotion(this));
                break;
        }
    }
}
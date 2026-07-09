

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



    public MovementStatemachineHandler(Action<float> HandleMovement,
    System.Action activeFallingHandling, System.Action HandleRotation, MovementHandler movementHandler,
    PlayerGroundCheck playerGroundCheck,
    MovementStateMachine movementStateMachine, PlayerData data) {
        this.HandleMovement = HandleMovement;
        this.activeFallingHandling = activeFallingHandling;
        this.HandleRotation = HandleRotation;
        this.movementHandler = movementHandler;
        this.movementStateMachine = movementStateMachine;
        this.playerGroundCheck = playerGroundCheck;
        this.data = data;
        this.HandleRotation = HandleRotation;
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
        movementStateMachine.ChangeState(new BasicLocomotion(movementHandler, ChangeRootState, CallbackChangeStateInObj,
        HandleMovement, HandleRotation, data, xPlayerPrimaryState.Movement.Locomotion.Idle));
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
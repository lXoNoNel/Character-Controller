using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class BasicLocomotion : IState
    {
        MovementHandler movementHandler;

        Action<float> HandleMovement;
        Action HandleRotation;
        public Action<IState> CallbackChangeRootState;
        public PlayerData data;
        public Action<xPlayerPrimaryState> CallbackChangeStateInObj;
        xPlayerPrimaryState basicLocoState;
        MovementStateMachine movementStateMachine;

        public BasicLocomotion(MovementHandler movementHandler, Action<IState> CallbackChangeRootState, Action<xPlayerPrimaryState> CallbackChangeStateInObj, 
        Action<float> HandleMovement, Action HandleRotation, PlayerData data, xPlayerPrimaryState basicLocoState)
        {
            this.movementHandler = movementHandler;
            this.CallbackChangeStateInObj = CallbackChangeStateInObj;
            this.HandleMovement = HandleMovement;
            this.HandleRotation = HandleRotation;
            this.data = data;
            this.basicLocoState = basicLocoState;
            this.CallbackChangeRootState = CallbackChangeRootState;

            movementStateMachine = new MovementStateMachine();
        }

        // public void ChangeRootState(IState movementState)
        // {
        //     this.movementStateMachine.ChangeState(movementState);
        // }

        public void changeIntoState(xPlayerPrimaryState state)
        {
            switch (state)
            {
                case var s when s == xPlayerPrimaryState.Movement.Locomotion.Idle:
                    movementStateMachine.ChangeState(new IdleLocomotion(this));
                    break;
                    
                case var s when s == xPlayerPrimaryState.Movement.Locomotion.Walking:
                    movementStateMachine.ChangeState(new WalkingLocomotion(this));
                    break;
                    
                case var s when s == xPlayerPrimaryState.Movement.Locomotion.Running:
                    movementStateMachine.ChangeState(new RunningLocomotion(this));
                    break;
                    
                case var s when s == xPlayerPrimaryState.Movement.Locomotion.Sprinting:
                    movementStateMachine.ChangeState(new SprintingLocomotion(this));
                    break;
            }
        }

        public override void Enter()
        {
            changeIntoState(basicLocoState);
        }


        public override void Execute()
        {
            HandleMovement(data.exactMovementState.MovementSpeed);
            HandleRotation();

            handleTransitions();
        }

        public override void Exit()
        {

        }

        void handleTransitions()
        {
            switch(data.exactMovementState)
            {
                #region IDLE TRANSITIONS

                case var s when s == xPlayerPrimaryState.Movement.Locomotion.Idle:
                    switch(movementHandler.detectedMovementStateByInput())
                    {
                        case MovementStateEnum.Walking:
                            movementStateMachine.ChangeState(new WalkingLocomotion(this));
                            break;
                        case MovementStateEnum.Running:
                            movementStateMachine.ChangeState(new RunningLocomotion(this));
                            break;
                        case MovementStateEnum.Sprinting:
                            movementStateMachine.ChangeState(new SprintingLocomotion(this));
                            break;
                    }
                    break;

                #endregion


                #region WALKING TRANSITIONS

                case var s when s == xPlayerPrimaryState.Movement.Locomotion.Walking:
                    switch(movementHandler.detectedMovementStateByInput())
                    {
                        case MovementStateEnum.Idle:
                            movementStateMachine.ChangeState(new IdleLocomotion(this));
                            break;
                        case MovementStateEnum.Running:
                            movementStateMachine.ChangeState(new RunningLocomotion(this));
                            break;
                        case MovementStateEnum.Sprinting:
                            movementStateMachine.ChangeState(new SprintingLocomotion(this));
                            break;
                    }
                    break;

                #endregion


                #region RUNNING TRANSITIONS

                case var s when s == xPlayerPrimaryState.Movement.Locomotion.Running:
                    switch(movementHandler.detectedMovementStateByInput())
                    {
                        case MovementStateEnum.Idle:
                            movementStateMachine.ChangeState(new IdleLocomotion(this));
                            break;
                        case MovementStateEnum.Walking:
                            movementStateMachine.ChangeState(new WalkingLocomotion(this));
                            break;
                        case MovementStateEnum.Sprinting:
                            movementStateMachine.ChangeState(new SprintingLocomotion(this));
                            break;
                    }
                    break;

                #endregion


                #region SPRINTING TRANSITIONS

                case var s when s == xPlayerPrimaryState.Movement.Locomotion.Sprinting:
                    switch(movementHandler.detectedMovementStateByInput())
                    {
                        case MovementStateEnum.Idle:
                            movementStateMachine.ChangeState(new IdleLocomotion(this));
                            break;
                        case MovementStateEnum.Walking:
                            movementStateMachine.ChangeState(new WalkingLocomotion(this));
                            break;
                        case MovementStateEnum.Running:
                            movementStateMachine.ChangeState(new RunningLocomotion(this));
                            break;
                    }
                    break;

                #endregion

                default:
                    break;
            }

        }

    }

}


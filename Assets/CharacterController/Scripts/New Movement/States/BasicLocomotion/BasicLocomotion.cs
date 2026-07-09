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
        public Action<MovementStateEnum> CallbackChangeStateInObj;
        BasicLocomotionStateEnum basicLocoState;
        MovementStateMachine movementStateMachine;

        public BasicLocomotion(MovementHandler movementHandler, Action<IState> CallbackChangeRootState, Action<MovementStateEnum> CallbackChangeStateInObj, Action<float> HandleMovement, Action HandleRotation, PlayerData data, BasicLocomotionStateEnum basicLocoState)
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

        public void changeIntoState(BasicLocomotionStateEnum state)
        {
            switch(state)
            {
                case BasicLocomotionStateEnum.Idle:
                    movementStateMachine.ChangeState(new IdleLocomotion(this));
                    break;
                case BasicLocomotionStateEnum.Walking:
                    movementStateMachine.ChangeState(new WalkingLocomotion(this));
                    break;
                case BasicLocomotionStateEnum.Running:
                    movementStateMachine.ChangeState(new RunningLocomotion(this));
                    break;
                case BasicLocomotionStateEnum.Sprinting:
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
            HandleMovement(data.movementState.GetSpeed());
            HandleRotation();

            handleTransitions();
        }

        public override void Exit()
        {

        }

        void handleTransitions()
        {
            switch(data.movementState)
            {
                #region IDLE TRANSITIONS

                case MovementStateEnum.Idle:
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

                case MovementStateEnum.Walking:
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

                case MovementStateEnum.Running:
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

                case MovementStateEnum.Sprinting:
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


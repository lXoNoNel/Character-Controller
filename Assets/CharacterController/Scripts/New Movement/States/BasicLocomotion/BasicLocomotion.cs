using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class BasicLocomotion : IState
    {
        MovementHandler movementHandler;

        Action<float> HandleMovement;
        Action<float> HandleMovement_End;
        Action HandleRotation;
        public Action<IState> CallbackChangeRootState;
        public PlayerData data;
        public Action<xPlayerPrimaryState> CallbackChangeStateInObj;

        public Action HandleMovement_Anim;

        xPlayerPrimaryState basicLocoState;
        MovementStateMachine movementStateMachine;

        public BasicLocomotion(MovementHandler movementHandler, Action<IState> CallbackChangeRootState, 
        Action<xPlayerPrimaryState> CallbackChangeStateInObj,  xPlayerPrimaryState basicLocoState)
        {
            this.movementHandler = movementHandler;
            this.CallbackChangeStateInObj = CallbackChangeStateInObj;
            this.CallbackChangeRootState = CallbackChangeRootState;

            this.basicLocoState = basicLocoState;

            this.HandleMovement = movementHandler.HandleMovement;
            this.HandleRotation = movementHandler.HandleRotation;
            this.data = movementHandler.data;


            this.HandleMovement_End = movementHandler.HandleMovement_End;
            this.HandleMovement_Anim = movementHandler.HandleMovement_Anim;

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
            // HandleMovement(data.exactMovementState.MovementSpeed);
            // HandleRotation();
            // HandleMovement_Anim();

            movementStateMachine.ExecuteStateUpdate();

            handleTransitions();
        }

        public override void FixedExecute()
        {
            movementStateMachine.ExecuteStateFixedUpdate();

            HandleMovement(data.exactMovementState.MovementSpeed);
            HandleRotation();
            HandleMovement_Anim();
        }

        public override void Exit()
        {
            this.HandleMovement_End(data.exactMovementState.MovementSpeed);
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

        //?========================================================================00
        

    }

}


using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class Airborne : IState
    {

        //? PLAYER GROUND CHECK BOOL
        //?========================================0
        PlayerGroundCheck playerGroundCheck;
        public bool isPlayerObjGrounded;

        //?========================================0
        
        public bool isInLandingAnimation;


        public MovementHandler movementHandler {get; private set;}


        //? CALLBACK STUFF
        //?========================================0
        public Action<IState> CallbackChangeRootState;
        public Action<xPlayerPrimaryState> CallbackChangeStateInObj;

        public Action locomotionStart;

        //?========================================0
        

        public PlayerData data;



        //? FUNCTION TO EXECUTE ON UPDATE
            public System.Action activeFallingHandling;
            public System.Action activeFallingHandling_OnExit;

        xPlayerPrimaryState airborneState;
        MovementStateMachine airborneStateMachine;

        MovementAnimationHandler movementAnimationHandler;



        public Airborne(MovementHandler movementHandler, MovementStatemachineHandler movementStatemachineHandler,
         xPlayerPrimaryState airborneState)
        {
            this.movementHandler = movementHandler;
            this.CallbackChangeStateInObj = movementStatemachineHandler.CallbackChangeStateInObj;
            this.data = movementHandler.data;
            this.CallbackChangeRootState = movementStatemachineHandler.ChangeRootState;

            // this.isPlayerObjGrounded = movementHandler.isPlayerObjGrounded();
            this.playerGroundCheck = movementStatemachineHandler.playerGroundCheck;

            this.airborneState = airborneState;
            this.locomotionStart = movementStatemachineHandler.locomotionStart;

            this.activeFallingHandling = movementStatemachineHandler.activeFallingHandling;
            this.activeFallingHandling_OnExit = movementHandler.activeFallingHandling_OnExit;

            this.movementAnimationHandler = movementHandler.movementAnimationHandler;

            airborneStateMachine = new MovementStateMachine();
        }

        public void ChangeRootState(IState movementState)
        {
            this.airborneStateMachine.ChangeState(movementState);
        }

        public void changeIntoState(xPlayerPrimaryState state)
        {
            switch (state)
            {
                case var s when s == xPlayerPrimaryState.Movement.Airborne.Falling:
                    ChangeRootState(new FallingAirborne(this, activeFallingHandling, activeFallingHandling_OnExit));
                    break;
                    
                case var s when s == xPlayerPrimaryState.Movement.Airborne.Jumping:
                    ChangeRootState(new JumpingAirborne(this));
                    break;
                    
                case var s when s == xPlayerPrimaryState.Movement.Airborne.Landing:
                    ChangeRootState(new LandingAirborne(this));
                    break;
            }
        }

        public override void Enter()
        {
            movementAnimationHandler.airborneEnter_Anim();

            changeIntoState(airborneState);
        }


        public override void Execute()
        {
            isPlayerObjGrounded = playerGroundCheck.isPlayerObjGrounded();

            airborneStateMachine.ExecuteStateUpdate();

            handleTransitions();
        }

        public override void Exit()
        {
            movementAnimationHandler.airborneExit_Anim();
        }

        private void handleTransitions()
        {
            switch (data.exactMovementState)
            {
                case var s when s == xPlayerPrimaryState.Movement.Airborne.Falling:
                    if(isPlayerObjGrounded)
                    {
                        ChangeRootState(new LandingAirborne(this));
                    }
                    break;
                    
                case var s when s == xPlayerPrimaryState.Movement.Airborne.Landing:
                    if(!movementAnimationHandler.isInLandingAnimation() || !movementAnimationHandler.isAnimatingAirborne)
                    {
                        locomotionStart();
                    }
                    break;
            }
        }


//?========================================================================00
        public override void FixedExecute()
        {
            
        }
    }

}


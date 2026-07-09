using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class FallingAirborne : IAirborneState
    {

        System.Action activeFallingHandling;
        System.Action activeFallingHandling_OnExit;

        public FallingAirborne(Airborne caller, System.Action activeFallingHandling, System.Action activeFallingHandling_OnExit) : base(caller)
        {
            this.activeFallingHandling = activeFallingHandling;
            this.activeFallingHandling_OnExit = activeFallingHandling_OnExit;
        }

        public FallingAirborne(MovementStatemachineHandler caller) : base(caller)
        {
        }

        public override void Enter()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Airborne.Falling;
            
            base.Enter();
        }

        public override void Execute()
        {
            base.Execute();

            activeFallingHandling();
        }

        public override void Exit()
        {
            base.Exit();

            activeFallingHandling_OnExit();
        }

    }

}


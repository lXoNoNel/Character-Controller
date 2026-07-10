using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class LandingAirborne : IAirborneState
    {

        public LandingAirborne(Airborne caller) : base(caller)
        {
        }

        public override void Enter()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Airborne.Landing;
            
            base.Enter();

            airborne.movementHandler.movementAnimationHandler.landingEnter_Anim();
        }


        public override void Exit()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Airborne.Landing;
            
            base.Enter();

            airborne.movementHandler.movementAnimationHandler.landingExit_Anim();
        }
    }

}


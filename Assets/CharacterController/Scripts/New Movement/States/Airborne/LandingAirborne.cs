using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class LandingAirborne : IAirborneState
    {

        public LandingAirborne(Airborne caller) : base(caller)
        {
        }

        public LandingAirborne(MovementStatemachineHandler caller) : base(caller)
        {
        }

        public override void Enter()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Airborne.Landing;
            
            base.Enter();
        }

    }

}


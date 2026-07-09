using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class JumpingAirborne : IAirborneState
    {

        public JumpingAirborne(Airborne caller) : base(caller)
        {
        }

        public JumpingAirborne(MovementStatemachineHandler caller) : base(caller)
        {
        }

        public override void Enter()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Airborne.Jumping;
            
            base.Enter();
        }

    }

}


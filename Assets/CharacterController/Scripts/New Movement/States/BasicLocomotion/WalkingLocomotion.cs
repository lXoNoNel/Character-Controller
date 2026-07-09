using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class WalkingLocomotion : LocoState
    {

        public WalkingLocomotion(BasicLocomotion caller) : base(caller)
        {
        }

        public WalkingLocomotion(MovementStatemachineHandler caller) : base(caller)
        {
        }

        public override void Enter()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Locomotion.Walking;
            
            base.Enter();
        }

    }

}


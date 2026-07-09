using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class SprintingLocomotion : LocoState
    {

        public SprintingLocomotion(BasicLocomotion caller) : base(caller)
        {
        }

        public SprintingLocomotion(MovementStatemachineHandler caller) : base(caller)
        {
        }

        public override void Enter()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Locomotion.Sprinting;
            
            base.Enter();
        }

    }

}


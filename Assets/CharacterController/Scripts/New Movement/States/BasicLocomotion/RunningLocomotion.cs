using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class RunningLocomotion : LocoState
    {

        public RunningLocomotion(BasicLocomotion caller) : base(caller)
        {
        }

        public RunningLocomotion(MovementStatemachineHandler caller) : base(caller)
        {
        }

        public override void Enter()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Locomotion.Running;
            
            base.Enter();
        }

    }

}


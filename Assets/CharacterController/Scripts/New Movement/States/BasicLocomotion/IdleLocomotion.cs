using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class IdleLocomotion : LocoState
    {

        public IdleLocomotion(BasicLocomotion caller) : base(caller)
        {
        }

        public IdleLocomotion(MovementStatemachineHandler caller) : base(caller)
        {
        }

        public override void Enter()
        {
            playerPrimaryState = xPlayerPrimaryState.Movement.Locomotion.Idle;
            
            base.Enter();
        }

    }

}


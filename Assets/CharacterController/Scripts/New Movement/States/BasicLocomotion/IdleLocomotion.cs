using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class IdleLocomotion : IState
    {
        Action<MovementStateEnum> CallbackChangeStateInObj;
        Action<IState> CallbackChangeRootState;


        public IdleLocomotion(BasicLocomotion caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.CallbackChangeRootState;
        }

        public IdleLocomotion(MovementStatemachineHandler caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.ChangeRootState;
        }

        public override void Enter()
        {
            this.CallbackChangeStateInObj(MovementStateEnum.Idle);
        }
        
        public override void Execute()
        {
        }

        public override void Exit()
        {
        }
    }

}


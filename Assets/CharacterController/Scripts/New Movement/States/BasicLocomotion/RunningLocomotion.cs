using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class RunningLocomotion : IState
    {
        Action<MovementStateEnum> CallbackChangeStateInObj;
        Action<IState> CallbackChangeRootState;


        public RunningLocomotion(BasicLocomotion caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.CallbackChangeRootState;
        }

        public RunningLocomotion(MovementStatemachineHandler caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.ChangeRootState;
        }

        public override void Enter()
        {
            this.CallbackChangeStateInObj(MovementStateEnum.Running);
        }
        
        public override void Execute()
        {

        }

        public override void Exit()
        {
        }
    }

}


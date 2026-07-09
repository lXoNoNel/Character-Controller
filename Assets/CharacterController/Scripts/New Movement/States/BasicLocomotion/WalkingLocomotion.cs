using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class WalkingLocomotion : IState
    {
        Action<MovementStateEnum> CallbackChangeStateInObj;
        Action<IState> CallbackChangeRootState;



        public WalkingLocomotion(BasicLocomotion caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.CallbackChangeRootState;
        }
        public WalkingLocomotion(MovementStatemachineHandler caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.ChangeRootState;
        }

        public override void Enter()
        {
            this.CallbackChangeStateInObj(MovementStateEnum.Walking);
        }
        
        public override void Execute()
        {
        }

        public override void Exit()
        {
        }
    }

}

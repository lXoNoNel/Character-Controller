using System;
using UnityEngine;


namespace MainGame.Movement.States
{
    public class SprintingLocomotion : IState
    {
        Action<MovementStateEnum> CallbackChangeStateInObj;
        Action<IState> CallbackChangeRootState;



        public SprintingLocomotion(BasicLocomotion caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.CallbackChangeRootState;
        }
        public SprintingLocomotion(MovementStatemachineHandler caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.ChangeRootState;
        }

        public override void Enter()
        {
            this.CallbackChangeStateInObj(MovementStateEnum.Sprinting);
        }
        
        public override void Execute()
        {

        }

        public override void Exit()
        {
        }
    }

}


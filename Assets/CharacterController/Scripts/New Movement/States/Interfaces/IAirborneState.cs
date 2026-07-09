using System;
using UnityEngine;

namespace MainGame.Movement.States
{
    public class IAirborneState : IState
    {
        protected Action<xPlayerPrimaryState> CallbackChangeStateInObj;
        protected Action<IState> CallbackChangeRootState;
        protected xPlayerPrimaryState playerPrimaryState;

        public IAirborneState(Airborne caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.CallbackChangeRootState;
        }

        public IAirborneState(MovementStatemachineHandler caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.ChangeRootState;
        }

        public override void Enter()
        {
            this.CallbackChangeStateInObj(playerPrimaryState);
        }
        public override void Execute()
        {
            
        }
        public override void Exit()
        {
            
        }
    }

}

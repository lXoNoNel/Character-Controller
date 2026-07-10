using System;
using UnityEngine;

namespace MainGame.Movement.States
{
    public class IAirborneState : IState
    {
        protected Action<xPlayerPrimaryState> CallbackChangeStateInObj;
        protected Action<IState> CallbackChangeRootState;
        protected xPlayerPrimaryState playerPrimaryState;
        
        protected Airborne airborne;

        public IAirborneState(Airborne caller)
        {
            this.CallbackChangeStateInObj = caller.CallbackChangeStateInObj;
            this.CallbackChangeRootState = caller.CallbackChangeRootState;

            this.airborne = caller;
        }

        public override void Enter()
        {
            this.CallbackChangeStateInObj(playerPrimaryState);
        }
        public override void Execute()
        {
            
        }

        public override void FixedExecute()
        {
            
        }

        public override void Exit()
        {
            
        }
    }

}

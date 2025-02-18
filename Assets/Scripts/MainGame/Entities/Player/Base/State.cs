using UnityEngine;

namespace Player.Base
{
    public abstract class State
    {
        protected PlayerBase player;
        protected StateMachine stateMachine;

        protected State(PlayerBase _player, StateMachine _stateMachine)
        {
            player = _player;
            stateMachine = _stateMachine;
        }

        public virtual void Enter() { }

        public virtual void HandleInput() { }

        public virtual void LogicUpdate() { }

        public virtual void PhysicsUpdate() { }

        public virtual void Exit() { }
    }
}

using UnityEngine;

namespace EnemyAI.Base
{
    public abstract class State
    {
        protected EnemyBase enemy;
        protected StateMachine stateMachine;

        public State(EnemyBase _enemy, StateMachine _stateMachine)
        {
            enemy = _enemy;
            stateMachine = _stateMachine;
        }

        public virtual void Enter() { }

        public virtual void LogicUpdate() { }

        public virtual void PhysicsUpdate() { }
        public virtual void Exit() { }
    }
}

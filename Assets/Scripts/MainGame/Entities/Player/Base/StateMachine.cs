using UnityEngine;

namespace Player.Base
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(State newState)
        {
            if (CurrentState != null)
                CurrentState.Exit();
            Debug.Log($"[StateMachine] Changing state to {newState.GetType().Name}");
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.StateMachine
{
    public class StateMachine<TState> : IDisposable where TState : class, IState
    {
        private Dictionary<Type, TState> _states;
        private TState _currentState;

        public void SetStates(List<TState> states)
        {
            _states = new Dictionary<Type, TState>();
            foreach (var state in states)
            {
                var type = state.GetType();
                if (_states.ContainsKey(type))
                    throw new ArgumentException($"Duplicate state type: {type}");
                _states.Add(type, state);
            }
        }
    
        public async void ChangeState<TTargetState>() where TTargetState : class, TState
        {
            var targetType = typeof(TTargetState);
            if (!_states.TryGetValue(targetType, out var nextState))
                throw new KeyNotFoundException($"State of type {targetType} is not registered.");

            if (_currentState != null && _currentState.GetType() == targetType)
                return;
            
            if (_currentState != null)
                await _currentState.Exit();
            _currentState = nextState;
            await _currentState.Enter();
        }
    
        public TTargetState GetCurrentState<TTargetState>() where TTargetState : class, TState
        {
            return _currentState as TTargetState;
        }

        public TState GetCurrentState()
        {
            return _currentState;
        }

        public void Dispose()
        {
            _currentState?.Exit();
            _states?.Clear();
            _currentState = null;
        }
    }
}

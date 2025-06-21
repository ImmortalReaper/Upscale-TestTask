using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.StateMachine;
using UnityEngine.EventSystems;

namespace Feature.UIModule.Scripts.MainMenuStateMachine
{
    public class MainMenuStateMachine : StateMachine<IMainMenuState>
    {
        public event Action<IMainMenuState> OnStateChanged;

        public override async Task ChangeState<TTargetState>()
        {
            await base.ChangeState<TTargetState>();
            OnStateChanged?.Invoke(GetCurrentState<TTargetState>());
        }

        public virtual async Task ChangeStateWithoutEnter(IMainMenuState targetState)
        {
            if (targetState == null)
                throw new ArgumentNullException(nameof(targetState), "Target state cannot be null.");

            if (_currentState != null && _currentState.GetType() == targetState.GetType())
                return;

            if (_currentState != null)
                await _currentState.Exit();
            _currentState = targetState;
            EventSystem.current.SetSelectedGameObject(targetState.Window.FirstSelectable);
        }
    
        public virtual async Task ChangeStateModal<TState>()
        {
            var targetType = typeof(TState);
            if (!_states.TryGetValue(targetType, out var nextState))
                throw new KeyNotFoundException($"State of type {targetType} is not registered.");

            if (_currentState != null && _currentState.GetType() == targetType)
                return;
        
            _currentState = nextState;
            await _currentState.Enter();
            OnStateChanged?.Invoke(GetCurrentState());
        }
    }
}

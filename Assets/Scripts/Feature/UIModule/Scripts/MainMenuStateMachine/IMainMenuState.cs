using Core.StateMachine;

namespace Feature.UIModule.Scripts.MainMenuStateMachine
{
    public interface IMainMenuState : IState
    {
        public BaseUIWindow Window { get; }
        public UIConfig WindowConfig { get; }
    }
}

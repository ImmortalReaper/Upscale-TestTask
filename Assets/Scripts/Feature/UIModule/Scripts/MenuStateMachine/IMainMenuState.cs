using Core.StateMachine;

namespace Feature.UIModule.Scripts.MenuStateMachine
{
    public interface IMainMenuState : IState
    {
        public BaseUIWindow Window { get; }
        public UIConfig WindowConfig { get; }
    }
}

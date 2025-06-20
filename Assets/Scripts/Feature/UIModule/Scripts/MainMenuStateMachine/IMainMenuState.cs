using Core.StateMachine;
using Feature.UIModule.Scripts;

public interface IMainMenuState : IState
{
    public BaseUIWindow Window { get; }
    public UIConfig WindowConfig { get; }
}

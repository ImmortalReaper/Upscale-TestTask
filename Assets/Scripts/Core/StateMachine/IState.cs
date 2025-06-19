using System.Threading.Tasks;

namespace Core.StateMachine
{
    public interface IState
    {
        public Task Enter();
        public Task Exit();
    }
}

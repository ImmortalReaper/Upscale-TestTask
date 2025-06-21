using System;

namespace Core.Input
{
    public interface IUIInputService
    {
        public event Action OnNext;
        public event Action OnPrevious;
        public event Action OnNextAlt;
        public event Action OnPreviousAlt;
        public event Action OnSubmit;
        public event Action OnCancel;
        public void Enable();
        public void Disable();
    }
}

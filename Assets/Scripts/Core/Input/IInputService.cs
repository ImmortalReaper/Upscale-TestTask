using System;
using UnityEngine;

namespace Core.Input
{
    public interface IInputService
    {
        public bool IsTapPressed { get; }
        public Vector2 TapPosition { get; }
        public bool IsHoldPressed { get; }
        public Vector2 HoldPosition { get; }
        
        public event Action<Vector2> OnTapPerformed;
        public event Action<Vector2> OnHoldStarted;
        public event Action<Vector2> OnHeld;
        public event Action<Vector2> OnHoldCanceled;

    }
}

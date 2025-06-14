using System;
using UnityEngine;

namespace Feature.UIModule.Scripts
{
    public abstract class BaseUIWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
    
        public bool IsVisible => _content.activeSelf;
        public event Action<BaseUIWindow> OnShow;
        public event Action<BaseUIWindow> OnHide;
    
        public virtual void Show()
        {
            _content.SetActive(true);
            OnShow?.Invoke(this);
        }

        public virtual void Hide()
        {
            _content.SetActive(false);
            OnHide?.Invoke(this);
        }
    }
}

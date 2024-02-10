using System;
using Code.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MVC
{
    public abstract class BaseMenuView : MonoBehaviour, IView
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private BoolButton _musicButton;
        [SerializeField] private BoolButton _soundButton;

        public Button RetryButton => _retryButton;
        public BoolButton MusicButton => _musicButton;
        public BoolButton SoundButton => _soundButton;

        public event Action OnDestroyView;

        protected void OnViewDestroyed()
        {
            OnDestroyView?.Invoke();
            RetryButton.onClick.RemoveAllListeners();
            OnDestroyView = null;
        }

        public virtual void SetTexts(string[] texts) { }
    }
}
using System;
using UnityEngine;

namespace Code.MVC
{
    public class OptionsView : MonoBehaviour, IView
    {
        [SerializeField] private RewardOptionView[] _rewardButtons;

        public RewardOptionView[] RewardButtons => _rewardButtons;

        public event Action OnDestroyView;

        public void UpdateOption(int index, bool isActive)
        {
            if (index < _rewardButtons.Length)
                RewardButtons[index].UpdateState(isActive);
            GameEventSystem.Send(new BackgroundEvent(index, isActive));
        }

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            OnDestroyView = null;
        }
    }
}
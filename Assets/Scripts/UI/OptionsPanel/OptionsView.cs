using GamePush;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MVC
{
    public class OptionsView : MonoBehaviour, IView
    {
        [SerializeField] private Button _openRoomButton;
        [SerializeField] private GameObject _roomPanel;
        [SerializeField] private RewardOptionView[] _rewardButtons;

        public RewardOptionView[] RewardButtons => _rewardButtons;

        public event Action OnDestroyView;

        private void Start()
        {
            var isMobile = GP_Device.IsMobile();
            _openRoomButton.gameObject.SetActive(isMobile);
            if (isMobile)
                _openRoomButton.onClick.AddListener(OpenRoom);
            _roomPanel.SetActive(false);
        }

        public void UpdateOption(int index, bool isActive)
        {
            if (index < _rewardButtons.Length)
                RewardButtons[index].UpdateState(isActive);
            GameEventSystem.Send(new BackgroundEvent(index, isActive));
        }

        private void OpenRoom() => _roomPanel.SetActive(true);

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            OnDestroyView = null;
            _openRoomButton.onClick.RemoveAllListeners();
        }
    }
}
using GamePush;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MVC
{
    public class OptionsView : MonoBehaviour, IView
    {
        [SerializeField] private Button _openRoomButton;
        [SerializeField] private GameObject _roomPanel;
        [SerializeField] private RewardOptionView[] _rewardButtons;
        private TextMeshProUGUI _buttonText;
        private bool _isMobile;

        public RewardOptionView[] RewardButtons => _rewardButtons;

        public event Action OnDestroyView;

        private void Start()
        {
            _isMobile = GP_Device.IsMobile();
            if (_isMobile)
            {
                _buttonText = _openRoomButton.GetComponent<TextMeshProUGUI>();
                _openRoomButton.gameObject.SetActive(true);
                _openRoomButton.onClick.AddListener(OpenRoom);
                _roomPanel.SetActive(false);
            }
        }

        public void UpdateOption(int index, bool isActive)
        {
            if (index < _rewardButtons.Length)
                RewardButtons[index].UpdateState(isActive);
            GameEventSystem.Send(new BackgroundEvent(index, isActive));
        }

        public void SetTexts(string[] texts)
        {
            if (_isMobile)
                _buttonText.text = texts[0];
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
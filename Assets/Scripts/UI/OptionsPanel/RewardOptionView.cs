using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Code.MVC
{
    public class RewardOptionView : MonoBehaviour
    {
        [SerializeField] private Image _activeImage;
        [SerializeField] private Image _lockedImage;
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private int _index;
        private TextMeshProUGUI _applyText;
        private TextMeshProUGUI _cancelText;

        public event Action<int, bool> OnChangeState;

        private void Awake()
        {
            _lockedImage.gameObject.SetActive(true);
            _applyButton.gameObject.SetActive(false);
            _cancelButton.gameObject.SetActive(false);
            _applyButton.onClick.AddListener(() => OnChangeState?.Invoke(_index, true));
            _cancelButton.onClick.AddListener(() => OnChangeState?.Invoke(_index, false));
            _applyText = _applyButton.GetComponentInChildren<TextMeshProUGUI>();
            _cancelText = _cancelButton.GetComponentInChildren<TextMeshProUGUI>();
            UpdateTextAsync();
        }

        public void UpdateState(bool isActive) => UpdateView(isActive);

        private void UpdateView(bool toApply)
        {
            _applyButton.gameObject.SetActive(!toApply);
            _cancelButton.gameObject.SetActive(toApply);
            _lockedImage.gameObject.SetActive(false);
        }

        private void UpdateTextAsync() =>
            LocalizationSettings.StringDatabase.GetTableAsync("TextTable").Completed +=
            handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var table = handle.Result;
                    _applyText.text = table.GetEntry("rewardApply")?.GetLocalizedString();
                    _cancelText.text = table.GetEntry("rewardRemove")?.GetLocalizedString();
                }
            };

        private void OnDestroy()
        {
            _applyButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            OnChangeState = null;
        }
    }
}
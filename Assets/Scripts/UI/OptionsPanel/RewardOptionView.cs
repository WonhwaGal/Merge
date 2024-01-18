using System;
using UnityEngine;
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

        public event Action<int, bool> OnChangeState;

        private void Awake()
        {
            _lockedImage.gameObject.SetActive(true);
            _applyButton.gameObject.SetActive(false);
            _cancelButton.gameObject.SetActive(false);
            _applyButton.onClick.AddListener(() => OnChangeState?.Invoke(_index, true));
            _cancelButton.onClick.AddListener(() => OnChangeState?.Invoke(_index, false));
        }

        public void UpdateState(bool isActive) => UpdateView(isActive);

        private void UpdateView(bool toApply)
        {
            _applyButton.gameObject.SetActive(!toApply);
            _cancelButton.gameObject.SetActive(toApply);
            _lockedImage.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _applyButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            OnChangeState = null;
        }
    }
}
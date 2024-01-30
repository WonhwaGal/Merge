using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Code.MVC
{
    public class StartCanvas : MonoBehaviour, IView
    {
        public Button StartNewButton;
        public Button ContinueButton;
        private TextMeshProUGUI _startText;
        private TextMeshProUGUI _continueText;

        public event Action OnDestroyView;

        private void Start()
        {
            _startText = StartNewButton.GetComponentInChildren<TextMeshProUGUI>();
            _continueText = ContinueButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetTexts(string[] texts)
        {
            _startText.text = texts[0];
            _continueText.text = texts[1];
        }

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            OnDestroyView = null;
            StartNewButton.onClick.RemoveAllListeners();
            ContinueButton.onClick.RemoveAllListeners();
        }
    }
}
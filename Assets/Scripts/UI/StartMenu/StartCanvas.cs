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
        public TMP_Dropdown LangDropDown;
        private TextMeshProUGUI _startText;
        private TextMeshProUGUI _continueText;

        public event Action OnDestroyView;

        private void Start()
        {
            _startText = StartNewButton.GetComponentInChildren<TextMeshProUGUI>();
            _continueText = ContinueButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetTexts(string startText, string continueText)
        {
            _startText.text = startText;
            _continueText.text = continueText;
        }

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            OnDestroyView = null;
            StartNewButton.onClick.RemoveAllListeners();
            ContinueButton.onClick.RemoveAllListeners();
            LangDropDown.onValueChanged.RemoveAllListeners();
        }
    }
}
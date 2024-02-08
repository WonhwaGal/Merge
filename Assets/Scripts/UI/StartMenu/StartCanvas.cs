using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GamePush;

namespace Code.MVC
{
    public class StartCanvas : MonoBehaviour, IView
    {
        public Button StartNewButton;
        public Button ContinueButton;
        public GameObject[] _objects;
        private TextMeshProUGUI _startText;
        private TextMeshProUGUI _continueText;

        public event Action OnDestroyView;

        private void Start()
        {
            _startText = StartNewButton.GetComponentInChildren<TextMeshProUGUI>();
            _continueText = ContinueButton.GetComponentInChildren<TextMeshProUGUI>();
            if (GP_Device.IsMobile())
            {
                for(int i = 0; i < _objects.Length; i++)
                    _objects[i].gameObject.SetActive(false);
            }
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
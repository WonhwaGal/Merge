using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using System;

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

        //private void Awake() => LocalizationSettings.InitializationOperation.WaitForCompletion();

        private void Start()
        {
            //LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
            //LangDropDown.onValueChanged.AddListener(ChangeLanguage);
            _startText = StartNewButton.GetComponentInChildren<TextMeshProUGUI>();
            _continueText = ContinueButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetTexts(string startText, string continueText)
        {
            _startText.text = startText;
            _continueText.text = continueText;
        }

        //private void OnSelectedLocaleChanged(Locale _) => UpdateTextAsync();

        //private void UpdateTextAsync() =>
        //    LocalizationSettings.StringDatabase.GetTableAsync("TextTable").Completed +=
        //        handle =>
        //        {
        //            if (handle.Status == AsyncOperationStatus.Succeeded)
        //            {
        //                var table = handle.Result;
        //                _startText.text = table.GetEntry("startB")?.GetLocalizedString();
        //                _continueText.text = table.GetEntry("continueB")?.GetLocalizedString();
        //            }
        //            else
        //            {
        //                string errorMessage = $"[{GetType().Name}] Could not load String Table: {handle.OperationException}";
        //                Debug.LogError(errorMessage);
        //            }
        //        };

        //private void ChangeLanguage(int index) 
        //    => LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];


        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            OnDestroyView = null;
            //LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
            StartNewButton.onClick.RemoveAllListeners();
            ContinueButton.onClick.RemoveAllListeners();
            LangDropDown.onValueChanged.RemoveAllListeners();
        }
    }
}
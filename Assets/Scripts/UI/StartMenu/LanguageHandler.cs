using System;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using GamePush;

namespace Code.MVC
{
    public class LanguageHandler
    {
        public event Action<string, string> OnLanguageChanged;

        public void UpdateLangInfo()
        {
            SetLanguage(ReceiveCurrentLang());
            UpdateTextAsync();
        }

        private void UpdateTextAsync() =>
            LocalizationSettings.StringDatabase.GetTableAsync("TextTable").Completed +=
                handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        var table = handle.Result;
                        OnLanguageChanged?.Invoke(table.GetEntry("startB")?.GetLocalizedString(),
                              table.GetEntry("continueB")?.GetLocalizedString());
                    }
                };

        private void SetLanguage(int index) 
            => LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        private int ReceiveCurrentLang() => GP_Language.Current() == Language.Russian ? 0 : 1;
    }
}
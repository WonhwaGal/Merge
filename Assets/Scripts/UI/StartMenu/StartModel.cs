using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using Code.DropLogic;
using Code.SaveLoad;

namespace Code.MVC
{
    public class StartModel: IModel
    {
        private readonly SaveService _saveService;
        private DropService _dropService;

        public StartModel()
        {
            LocalizationSettings.InitializationOperation.WaitForCompletion();
            _saveService = ServiceLocator.Container.RequestFor<SaveService>();
        }

        public event Action<string, string> OnLanguageChanged;

        public void Init() => LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;

        public void ChangeLanguage(int index)
            => LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        private void OnSelectedLocaleChanged(Locale _) => UpdateTextAsync();

        private void UpdateTextAsync() =>
            LocalizationSettings.StringDatabase.GetTableAsync("TextTable").Completed +=
                handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        var table = handle.Result;
                        OnLanguageChanged?.Invoke(table.GetEntry("startB")?.GetLocalizedString(),
                            table.GetEntry("continueB")?.GetLocalizedString());
                        //_startText.text = table.GetEntry("startB")?.GetLocalizedString();
                        //_continueText.text = table.GetEntry("continueB")?.GetLocalizedString();
                    }
                    else
                    {
                        string errorMessage = $"[{GetType().Name}] Could not load String Table: {handle.OperationException}";
                        Debug.LogError(errorMessage);
                    }
                };

        public void LoadNewScene(bool withProgress)
        {
            SceneManager.LoadSceneAsync(Constants.GameScene);
            if (withProgress)
                SceneManager.sceneLoaded += OnLoadWithProgress;
        }

        private void OnLoadWithProgress(Scene scene, LoadSceneMode mode)
        {
            _dropService = ServiceLocator.Container.RequestFor<DropService>();
            _dropService.RecreateProgress(_saveService.ProgressData);
            SceneManager.sceneLoaded -= OnLoadWithProgress;
        }

        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
        }
    }
}
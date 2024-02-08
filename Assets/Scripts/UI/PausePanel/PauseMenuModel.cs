using System;
using Code.Achievements;
using Code.SaveLoad;
using GamePush;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.MVC
{
    public sealed class PauseMenuModel : IModel
    {
        private readonly SaveService _saveService;
        private AchievementService _achievementService;

        public PauseMenuModel()
        {
            GameEventSystem.Subscribe<SaveEvent>(OnSaveData);
            _saveService = ServiceLocator.Container.RequestFor<SaveService>();
        }

        public float BestScore => GP_Player.GetScore();
        public GameAction LastAction { get; set; }

        public event Action<string[]> OnLanguageChanged;

        public void Init()
        {
            UpdateTextAsync();
            _achievementService = ServiceLocator.Container.RequestFor<AchievementService>();
        }

        public void OpenAchievements() => _achievementService.Open();

        public void OnSaveData(SaveEvent @event)
        {
            if (LastAction == GameAction.Lose && !@event.OnlyScore)
            {
                _saveService.ClearData();
                return;
            }

            if (!@event.OnlyScore)
                GameEventSystem.Send(new GameControlEvent(GameAction.Save, false));
            _saveService.SaveData(@event.CurrentScore, @event.OnlyScore);
        }

        public void PressRetry() => GameEventSystem
            .Send(new GameControlEvent(GameAction.Play, restartWithRetry: true));

        public bool GetVolume(SoundType type)
        {
            if (type == SoundType.TotalMusic)
                return GP_Player.GetBool(Constants.TotalMusic);
            else
                return GP_Player.GetBool(Constants.TotalSound);
        }

        public void UpdateTextAsync() =>
            LocalizationSettings.StringDatabase.GetTableAsync("TextTable").Completed +=
                handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        var table = handle.Result;
                        OnLanguageChanged?.Invoke(new string[6] {table.GetEntry("loseText")?.GetLocalizedString(),
                            table.GetEntry("bestScoreText")?.GetLocalizedString(),
                            table.GetEntry("retryB")?.GetLocalizedString(),
                            table.GetEntry("optionsB")?.GetLocalizedString(),
                            table.GetEntry("achievsB")?.GetLocalizedString(),
                            table.GetEntry("roomB")?.GetLocalizedString()});
                    }
                };

        public void Dispose()
        {
            GameEventSystem.UnSubscribe<SaveEvent>(OnSaveData);
            GC.SuppressFinalize(this);
        }
    }
}
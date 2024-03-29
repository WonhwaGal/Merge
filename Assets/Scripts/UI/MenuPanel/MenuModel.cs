﻿using System;
using Code.SaveLoad;
using GamePush;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.MVC
{
    public sealed class MenuModel : IModel
    {
        private readonly SaveService _saveService;

        public MenuModel()
        {
            GameEventSystem.Subscribe<SaveEvent>(OnSaveData);
            _saveService = ServiceLocator.Container.RequestFor<SaveService>();
        }

        public float BestScore => GP_Player.GetScore();
        public GameAction LastAction { get; set; }

        public event Action<string, string, string> OnLanguageChanged;

        public void Init() => UpdateTextAsync();

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

        private void UpdateTextAsync() =>
            LocalizationSettings.StringDatabase.GetTableAsync("TextTable").Completed +=
                handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        var table = handle.Result;
                        OnLanguageChanged?.Invoke(table.GetEntry("loseText")?.GetLocalizedString(),
                            table.GetEntry("bestScoreText")?.GetLocalizedString(),
                            table.GetEntry("retryB")?.GetLocalizedString());
                    }
                };

        public void Dispose()
        {
            GameEventSystem.UnSubscribe<SaveEvent>(OnSaveData);
            GC.SuppressFinalize(this);
        }
    }
}
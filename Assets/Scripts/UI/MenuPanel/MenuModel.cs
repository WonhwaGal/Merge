using System;
using Code.SaveLoad;
using UnityEngine;

namespace Code.MVC
{
    public sealed class MenuModel : IModel
    {
        private readonly SaveService _saveService;

        public MenuModel()
        {
            GameEventSystem.Subscribe<SaveEvent>(OnSaveData);
            _saveService = ServiceLocator.Container.RequestFor<SaveService>();
            GetBestScore();
        }

        public int BestScore { get; private set; }
        public int CurrentScore { get; private set; }
        public GameAction LastAction { get; set; }

        public void OnSaveData(SaveEvent @event)
        {
            if (LastAction == GameAction.Lose && !@event.OnlyScore)
            {
                Debug.Log("request for save denied, last action is " + LastAction);
                _saveService.ClearData();
                return;
            }

            if (!@event.OnlyScore)
                GameEventSystem.Send(new GameControlEvent(GameAction.Save, false));
            _saveService.SaveData(@event.CurrentScore, @event.OnlyScore);
        }

        public void GetBestScore()
        {
            BestScore = _saveService.ProgressData == null ?
                0 : _saveService.ProgressData.BestScore;
            CurrentScore = _saveService.ProgressData == null ?
                0 : _saveService.ProgressData.SavedScore;
        }

        public void Dispose()
        {
            GameEventSystem.UnSubscribe<SaveEvent>(OnSaveData);
            GC.SuppressFinalize(this);
        }
    }
}
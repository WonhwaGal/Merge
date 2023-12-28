using System;
using Code.SaveLoad;
using GamePush;
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
            GetScores();
        }

        public float BestScore { get; private set; }
        public float CurrentScore { get; private set; }
        public GameAction LastAction { get; set; }

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

        public void GetScores()
        {
            CurrentScore = GP_Player.GetScore();
            BestScore = GP_Player.GetInt("best_score");
        }
        //public void GetBestScore()
        //{
        //    BestScore = _saveService.ProgressData == null ?
        //        0 : _saveService.ProgressData.BestScore;
        //    CurrentScore = _saveService.ProgressData == null ?
        //        0 : _saveService.ProgressData.SavedScore;
        //}

        public void Dispose()
        {
            GameEventSystem.UnSubscribe<SaveEvent>(OnSaveData);
            GC.SuppressFinalize(this);
        }
    }
}
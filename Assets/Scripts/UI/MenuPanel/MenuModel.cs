using System;
using Code.SaveLoad;
using GamePush;

namespace Code.MVC
{
    public sealed class MenuModel : IModel
    {
        private readonly SaveService _saveService;

        public MenuModel()
        {
            GameEventSystem.Subscribe<SaveEvent>(OnSaveData);
            _saveService = ServiceLocator.Container.RequestFor<SaveService>();
            BestScore = GP_Player.GetScore();
        }

        public float BestScore { get; private set; }
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

        public void PressRetry() => GameEventSystem
            .Send(new GameControlEvent(GameAction.Play, restartWithRetry: true));

        public void Dispose()
        {
            GameEventSystem.UnSubscribe<SaveEvent>(OnSaveData);
            GC.SuppressFinalize(this);
        }
    }
}
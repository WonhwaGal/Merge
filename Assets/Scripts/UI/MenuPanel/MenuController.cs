using System;
using UnityEngine;

namespace Code.MVC
{
    public sealed class MenuController : Controller<MenuView, MenuModel>
    {
        private GameAction _gameAction;

        public MenuController() : base()
        {
            GameEventSystem.Subscribe<GameControlEvent>(SetUpPanel);
        }

        public Func<float> OnRequestScore;

        private void SetUpPanel(GameControlEvent @event)
        {
            _gameAction = @event.ActionToDo;
            Model.LastAction = _gameAction;
            UpdateView();
        }

        public override void UpdateView()
        {
            UpdateTimeScale();
            if (_gameAction == GameAction.Lose || _gameAction == GameAction.Pause)
                Show();
            else if (_gameAction == GameAction.Play)
                Hide();
        }

        private void UpdateTimeScale()
        {
            if (_gameAction == GameAction.Save)
                return;

            Time.timeScale = _gameAction == GameAction.Play ? 1 : 0;
        }

        protected override void Hide() => View.gameObject.SetActive(false);
        protected override void Show()
        {
            View.BestScore = Model.BestScore;
            View.FinalScore = OnRequestScore.Invoke();
            View.gameObject.SetActive(true);
            View.ShowResults(_gameAction == GameAction.Lose, Model.BestScore);
        }

        public override void Dispose()
        {
            OnRequestScore = null;
            GameEventSystem.UnSubscribe<GameControlEvent>(SetUpPanel);
            base.Dispose();
        }
    }
}
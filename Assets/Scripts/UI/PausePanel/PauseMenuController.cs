using System;
using UnityEngine;
using GamePush;

namespace Code.MVC
{
    public sealed class PauseMenuController : Controller<PauseMenuView, PauseMenuModel>
    {
        private GameAction _gameAction;

        public PauseMenuController() : base()
        {
            GameEventSystem.Subscribe<GameControlEvent>(SetUpPanel);
        }

        public Func<float> OnRequestScore;
        public event Action OnRequestRewards;

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
            View.FinalScore = OnRequestScore.Invoke();
            View.gameObject.SetActive(true);
            View.ShowResults(_gameAction == GameAction.Lose, Model.BestScore);
        }

        protected override void OnViewAdded()
        {
            View.RetryButton.onClick.AddListener(Model.PressRetry);
            View.RewardsButton.onClick.AddListener(() => OnRequestRewards?.Invoke());
            View.AchievementButton.onClick.AddListener(Model.OpenAchievements);
            View.MusicButton.SetBool(Model.GetVolume(SoundType.TotalMusic));
            View.SoundButton.SetBool(Model.GetVolume(SoundType.TotalSound));
            Model.OnLanguageChanged += View.SetTexts;
            SetUpBakeryPanel();
            Model.Init();
        }

        private void SetUpBakeryPanel()
        {
            if (GP_Device.IsMobile())
            {
                View.Room.SetActive(false);
                View.BakeryButton.onClick.AddListener(() => View.Room.SetActive(true));
            }
            else
            {
                View.BakeryButton.gameObject.SetActive(false);
            }
        }

        public override void Dispose()
        {
            OnRequestScore = null;
            OnRequestRewards = null;
            GameEventSystem.UnSubscribe<GameControlEvent>(SetUpPanel);
            Model.OnLanguageChanged -= View.SetTexts;
            base.Dispose();
        }
    }
}
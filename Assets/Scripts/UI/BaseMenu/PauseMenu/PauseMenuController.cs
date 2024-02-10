using GamePush;
using System;

namespace Code.MVC
{
    public class PauseMenuController : BaseMenuController<PauseMenuView, PauseMenuModel>
    {
        public PauseMenuController() : base() => _triggerAction = GameAction.Pause;

        public event Action OnRequestRewards;

        protected override void Show()
        {
            View.FinalScore = OnRequestScore.Invoke();
            View.gameObject.SetActive(true);
            View.ShowContent();
        }

        protected override void InitComponents()
        {
            View.RewardsButton.onClick.AddListener(() => OnRequestRewards?.Invoke());
            View.AchievementButton.onClick.AddListener(Model.OpenAchievements);
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

        protected override void OnDispose() => OnRequestRewards = null;
    }
}
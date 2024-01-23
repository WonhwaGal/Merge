
namespace Code.MVC
{
    public class OptionController : Controller<OptionsView, OptionsModel>
    {
        public OptionController(AchievSO so) : base()
        {
            Model.Init(so);
            GameEventSystem.Subscribe<GameControlEvent>(SetUpPanel);
        }

        public override void UpdateView()
        {
            if (View.gameObject.activeSelf)
                Hide();
            else
                Show();
        }

        protected override void Hide() => View.gameObject.SetActive(false);

        protected override void Show()
        {
            View.gameObject.SetActive(true);
            Model.UpdateStates();
        }

        protected override void OnViewAdded()
        {
            Model.OnUpdateReward += View.UpdateOption;
            Model.OnLanguageChanged += View.SetText;
            for (int i = 0; i < View.RewardButtons.Length; i++)
                View.RewardButtons[i].OnChangeState += Model.OnChangeRewardState;
        }

        private void SetUpPanel(GameControlEvent @event)
        {
            if (!View.gameObject.activeSelf)
                return;
            if (@event.ActionToDo == GameAction.Play)
                UpdateView();
        }

        public override void Dispose()
        {
            GameEventSystem.UnSubscribe<GameControlEvent>(SetUpPanel);
            Model.Dispose();
            base.Dispose();
        }
    }
}
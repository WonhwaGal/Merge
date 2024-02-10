using System;

namespace Code.MVC
{
    public class LoseMenuController : BaseMenuController<LoseMenuView, LoseMenuModel>
    {
        public LoseMenuController() : base() => _triggerAction = GameAction.Lose;

        protected override void Show()
        {
            View.FinalScore = OnRequestScore.Invoke();
            View.gameObject.SetActive(true);
            View.ShowResults(Model.BestScore);
        }

        protected override void InitComponents() => Model.Init();
    }
}
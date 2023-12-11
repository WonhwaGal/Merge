using System;

namespace Code.MVC
{
    public sealed class MenuController : Controller<MenuView, MenuModel>
    {
        private bool _isOnPause;

        public bool GameIsLost { get; private set; }

        public Func<int> OnRequestScore;

        public override void UpdateView()
        {
            Show();
            View.BestScore =  Model.BestScore;
            View.FinalScore = OnRequestScore.Invoke();
        }

        public void ShowPauseView(bool isPaused)
        {
            _isOnPause = isPaused;
            View.FinalScore = OnRequestScore.Invoke();
            if (_isOnPause)
                Show();
            else
                Hide();
        }

        public void AssignView() => View.OnSaveProgress += Model.OnSaveData;
        protected override void Hide() => View.gameObject.SetActive(false);
        protected override void Show()
        {
            View.gameObject.SetActive(true);
            View.ShowResults(!_isOnPause, Model.BestScore);
            GameIsLost = !_isOnPause;
        }
    }
}
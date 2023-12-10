using System;

namespace Code.MVC
{
    public class MenuController : Controller<PauseView, PauseModel>
    {
        private bool _isOnPause;

        public Func<int> OnRequestScore;

        public override void UpdateView()
        {
            View.FinalScore = OnRequestScore.Invoke();
            Show();
        }

        public void ShowPauseView(bool isPaused)
        {
            _isOnPause = isPaused;
            if (_isOnPause)
                Show();
            else
                Hide();
        }

        protected override void Hide() => View.gameObject.SetActive(false);
        protected override void Show()
        {
            View.gameObject.SetActive(true);
            View.ShowResults(!_isOnPause);
        }
    }
}
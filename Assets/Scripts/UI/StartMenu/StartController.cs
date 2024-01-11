
namespace Code.MVC
{
    public sealed class StartController : Controller<StartCanvas, StartModel>
    {
        public StartController() : base() => Model.Init();

        public override void UpdateView() { }

        protected override void OnViewAdded()
        {
            View.StartNewButton.onClick.AddListener(() => Model.LoadNewScene(withProgress: false));
            View.ContinueButton.onClick.AddListener(() => Model.LoadNewScene(withProgress: true));
            View.LangDropDown.onValueChanged.AddListener(Model.ChangeLanguage);
            Model.OnLanguageChanged += View.SetTexts;
        }

        protected override void Hide() => View.gameObject.SetActive(false);
        protected override void Show() => View.gameObject.SetActive(true);

        public override void Dispose()
        {
            Model.OnLanguageChanged -= View.SetTexts;
            Model.Dispose();
            base.Dispose();
        }
    }
}
using Code.Achievements;
using UnityEngine.Localization.Tables;

namespace Code.MVC
{
    public class PauseMenuModel : BaseMenuModel
    {
        private AchievementService _achievementService;

        public PauseMenuModel() : base() { }

        public void Init()
        {
            UpdateTextAsync();
            _achievementService = ServiceLocator.Container.RequestFor<AchievementService>();
        }

        public void OpenAchievements() => _achievementService.Open();

        protected override string[] GetEntries(StringTable table)
        {
            return new string[4] {table.GetEntry("retryB")?.GetLocalizedString(),
                            table.GetEntry("optionsB")?.GetLocalizedString(),
                            table.GetEntry("achievsB")?.GetLocalizedString(),
                            table.GetEntry("roomB")?.GetLocalizedString()};
        }
    }
}
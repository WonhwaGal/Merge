using GamePush;
using UnityEngine.Localization.Tables;

namespace Code.MVC
{
    public class LoseMenuModel : BaseMenuModel
    {
        public float BestScore => GP_Player.GetScore();

        public void Init() => UpdateTextAsync();

        protected override string[] GetEntries(StringTable table)
        {
            return new string[3] {table.GetEntry("loseText")?.GetLocalizedString(),
                            table.GetEntry("bestScoreText")?.GetLocalizedString(),
                            table.GetEntry("retryB")?.GetLocalizedString()};
        }
    }
}
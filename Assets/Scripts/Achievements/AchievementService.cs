using System;
using static AchievSO.AchievBlock;
using GamePush;


namespace Code.Achievements
{
    public sealed class AchievementService : IService
    {
        private readonly AchievSO _achievSO;

        public AchievementService(AchievSO so) => _achievSO = so;

        public void Open() => GP_Achievements.Open();

        public void CheckAchievement(AchievType type, float referenceValue)
        {
            var achievBlock = _achievSO.FindAchiev(type);
            if (achievBlock == null)
                return;

            for (int i = 0; i < achievBlock.Achievements.Count; i++)
            {
                var achiev = achievBlock.Achievements[i];
                if (achiev.IsUnlocked)
                    continue;

                CheckAchievement(achiev, type == AchievType.MergeByRank, referenceValue);
            }
        }

        public void SetProgress(bool toZero)
        {
            for (int i = 0; i < _achievSO.AchievsByType.Count; i++)
            {
                var achievList = _achievSO.AchievsByType[i].Achievements;
                for (int ach = 0; ach < achievList.Count; ach++)
                {
                    var achiev = _achievSO.AchievsByType[i].Achievements[ach];
                    if (achiev.IsUnlocked)
                        continue;

                    var id = achiev.AchievID.ToString();
                    if (toZero && !achiev.IsTotal)
                        GP_Achievements.SetProgress(id, 0);
                    else if (achiev.IsTotal)
                        achiev.MergeTimes = GP_Achievements.GetProgress(id);
                }
            }
        }
        private void SetProgress(bool isMergeByRank, Achievement achiev, float referenceValue)
        {
            if (!isMergeByRank)
                GP_Achievements.SetProgress(achiev.AchievID.ToString(), (int)referenceValue);
        }

        private void CheckAchievement(Achievement achiev, bool isMerge, float referenceValue)
        {
            SetProgress(isMerge, achiev, referenceValue);

            if (isMerge && referenceValue == achiev.ReferenceValue)
            {
                HandleMergeAchiev(achiev);
                return;
            }
            else if (!isMerge && referenceValue >= achiev.ReferenceValue)
            {
                UnlockAchievement(achiev);
                return;
            }
        }

        private void HandleMergeAchiev(Achievement achiev)
        {
            var progressValue = ++achiev.MergeTimes;
            GP_Achievements.SetProgress(achiev.AchievID.ToString(), progressValue);
            if (progressValue >= achiev.Condition)
                UnlockAchievement(achiev);
        }

        private void UnlockAchievement(Achievement achiev)
        {
            var id = achiev.AchievID.ToString();
            GP_Achievements.Unlock(id);
            achiev.IsUnlocked = true;
        }
    }
}